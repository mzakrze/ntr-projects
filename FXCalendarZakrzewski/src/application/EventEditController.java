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

	private static EventManager eventManager = EventManager.getInstance();
	public final static String EVENT_ID = "EVENT_ID";

	public void onSubmitButtonClicked(MouseEvent event){
		Integer id = (Integer) ((Node) event.getSource()).getParent().getProperties().get(EVENT_ID);
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
		if(beginTime.isAfter(endTime)){
			isValid = false;
		}
		if(name == null || name.isEmpty()){
			isValid = false;
		}
		if(isValid){
			Event usersEvent = new Event(id, date, name, description, beginTime, endTime);
			eventManager.upsert(usersEvent); // TODO coś nie działa, bo przy przewijaniu exception
			// TODO odwierzyc widok
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


	class EventValidationResult{
		public Map<Node, String> elementErrorMsgMap = new HashMap<>();
	}
}
