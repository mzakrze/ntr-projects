package application;

import java.time.LocalDate;
import java.time.LocalTime;

import javafx.fxml.FXML;
import javafx.scene.Node;
import javafx.scene.control.DatePicker;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseEvent;
import javafx.stage.Stage;

public class EventEditController {
	public static final String datePickerId = "datePicker";
	public static final String startTimeHoursId = "startTimeHours";
	public static final String startTimeMinutesId = "startTimeMinutes";
	public static final String endTimeHoursId = "endTimeHours";
	public static final String endTimeMinutesId = "endTimeMinutes";
	public static final String eventNameId = "eventName";
	public static final String eventDescriptionId = "eventDescription";
	public final static String EVENT_ID = "EVENT_ID";

	private static EventManager eventManager = EventManager.getInstance();

	{
		ComponentsManager.getInstance().registerComponentAsEventEditController(this);
	}

	@FXML private DatePicker datePicker;
	@FXML private TextField startTimeHours;
	@FXML private TextField startTimeMinutes;
	@FXML private TextField endTimeHours;
	@FXML private TextField endTimeMinutes;
	@FXML private TextField eventName;
	@FXML private TextArea eventDescription;

	private Integer currentryEditedEventId;

	public EventEditController(){

	}

	public void setCurrentlyEditedEventId(Integer eventId){
		this.currentryEditedEventId = eventId;
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
			reloadCalendar();
			closeWindow(event);
		}
	}

	public void onCancelButtonClicked(MouseEvent event){
		((Stage)((Node) event.getSource()).getParent().getScene().getWindow()).close();
	}

	public void onDeleteButtonClicked(MouseEvent event){
		eventManager.deleteById(currentryEditedEventId);
		reloadCalendar();
		closeWindow(event);
	}

	private void closeWindow(MouseEvent event){
		((Stage)((Node) event.getSource()).getParent().getScene().getWindow()).close();
	}

	private void reloadCalendar(){
		ComponentsManager.getInstance().getCalendarController().requestReload();
	}
}
