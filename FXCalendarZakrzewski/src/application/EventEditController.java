package application;

import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.ZoneId;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

import javafx.fxml.FXML;
import javafx.scene.Node;
import javafx.scene.control.Button;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.stage.Stage;

public class EventEditController {
	@FXML DatePicker datePicker;
	@FXML TextField startTimeHours;
	@FXML TextField startTimeMinutes;
	@FXML TextField endTimeHours;
	@FXML TextField endTimeMinutes;
	@FXML TextField eventName;
	@FXML TextArea eventDescription;
	public static final String datePickerId = "datePicker";
	public static final String startTimeHoursId = "startTimeHours";
	public static final String startTimeMinutesId = "startTimeMinutes";
	public static final String endTimeHoursId = "endTimeHours";
	public static final String endTimeMinutesId = "endTimeMinutes";
	public static final String eventNameId = "eventName";
	public static final String eventDescriptionId = "eventDescription";
	public final static String EVENT_ID = "EVENT_ID";

	private Integer currentryEditedEventId;

	public void setCurrentlyEditedEventId(Integer eventId){
		this.currentryEditedEventId = eventId;
	}

	private static EventManager eventManager = EventManager.getInstance();


	public EventEditController(){
		ComponentsManager.getInstance().registerComponentAsEventEditController(this);
	}

	public void onSubmitButtonClicked(MouseEvent event){
		LocalDate date = datePicker.getValue();
		String startTimeHoursStr = startTimeHours.getText();
		String startTimeMinutesStr = startTimeMinutes.getText();
		String endTimeHoursStr = endTimeHours.getText();
		String endTimeMinutesStr = endTimeMinutes.getText();
		String name = eventName.getText();
		String description = eventDescription.getText();
		LocalTime beginTime = null;
		LocalTime endTime = null;
		boolean isValid = true;
		try{
			beginTime = LocalTime.of(Integer.valueOf(startTimeHoursStr), Integer.valueOf(startTimeMinutesStr));
			endTime = LocalTime.of(Integer.valueOf(endTimeHoursStr), Integer.valueOf(endTimeMinutesStr));
		} catch(Throwable t){
			isValid = false;
		}
		if(beginTime == null || beginTime.isAfter(endTime)){
			isValid = false;
		}
		if(name == null || name.isEmpty()){
			isValid = false;
		}
		if(isValid){
			Event usersEvent = new Event(currentryEditedEventId, date, name, description, beginTime, endTime);
			eventManager.upsert(usersEvent);
			ComponentsManager.getInstance().getCalendarController().requestReload();
			((Stage)((Node) event.getSource()).getParent().getScene().getWindow()).close();
		} else {
			System.out.println("validation failed");
		}
	}

	private Map<Node, String> validateEvent(Event event) {
		Map<Node, String> toReturn = new HashMap<>();
		if(event.getBeginTime().isAfter(event.getEndTime())){
			toReturn.put(endTimeHours, "Kurwa mać");
			toReturn.put(endTimeMinutes, "Jebane gówno");
		}
		if(event.getName() == null || event.getName().isEmpty()){
			toReturn.put(eventName, "Please enter event name");
		}

		return toReturn;
	}

	public void onCancelButtonClicked(MouseEvent event){
		System.out.println("cancel");
		((Stage)((Node) event.getSource()).getParent().getScene().getWindow()).close();
	}

	public void onDeleteButtonClicked(MouseEvent event){
		eventManager.deleteById(currentryEditedEventId);
		ComponentsManager.getInstance().getCalendarController().requestReload();
		((Stage)((Node) event.getSource()).getParent().getScene().getWindow()).close();
	}


	class EventValidationResult{
		public Map<Node, String> elementErrorMsgMap = new HashMap<>();
	}
}
