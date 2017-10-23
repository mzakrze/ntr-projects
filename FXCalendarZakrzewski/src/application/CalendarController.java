package application;

import java.io.IOException;
import java.lang.reflect.Field;
import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.time.temporal.WeekFields;
import java.util.List;
import java.util.Locale;

import com.sun.xml.internal.bind.v2.WellKnownNamespace;

import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.VBox;
import javafx.scene.text.Font;
import javafx.stage.Modality;
import javafx.stage.Stage;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseButton;
import javafx.scene.input.MouseEvent;

public class CalendarController {
	private final static String CALENDAR_CARD_DAY = "CALENDAR_CARD_DAY";
	private final static String EVENT_ID = "EVENT_ID";

	// vbox<wiersz><kolumna>
	@FXML private VBox vbox11;
	@FXML private VBox vbox21;
	@FXML private VBox vbox31;
	@FXML private VBox vbox41;
	@FXML private VBox vbox12;
	@FXML private VBox vbox22;
	@FXML private VBox vbox32;
	@FXML private VBox vbox42;
	@FXML private VBox vbox13;
	@FXML private VBox vbox23;
	@FXML private VBox vbox33;
	@FXML private VBox vbox43;
	@FXML private VBox vbox14;
	@FXML private VBox vbox24;
	@FXML private VBox vbox34;
	@FXML private VBox vbox44;
	@FXML private VBox vbox15;
	@FXML private VBox vbox25;
	@FXML private VBox vbox35;
	@FXML private VBox vbox45;
	@FXML private VBox vbox16;
	@FXML private VBox vbox26;
	@FXML private VBox vbox36;
	@FXML private VBox vbox46;
	@FXML private VBox vbox17;
	@FXML private VBox vbox27;
	@FXML private VBox vbox37;
	@FXML private VBox vbox47;

	// week<nr_tygodnia><L/P>
	@FXML private Label week1L;
	@FXML private Label week1P;
	@FXML private Label week2L;
	@FXML private Label week2P;
	@FXML private Label week3L;
	@FXML private Label week3P;
	@FXML private Label week4L;
	@FXML private Label week4P;

	// <prev/next><U/D>
	@FXML private Label prevU;
	@FXML private Label prevD;
	@FXML private Label nextU;
	@FXML private Label nextD;

	private LocalDate today = LocalDate.now();
	private LocalDate firstDayInCalendar;
	private EventManager eventManager = EventManager.getInstance();

	{
		ComponentsManager.getInstance().registerComponentAsCalendarController(this);
	}

	public CalendarController(){
		//ComponentsManager.getInstance().registerComponentAsCalendarController(this);
	}

	@FXML
    public void initialize() {
		calculatefirstDayInCalendar(today);
		fillCalendar();
    }

	public void requestReload() {
		fillCalendar();
	}

	public void onPrevClickedMethod(javafx.event.Event ev){
		moveFirstDayInCalendarWeekBackwards();
		fillCalendar();
	}

	public void onNextClickedMethod(javafx.event.Event event){
		moveFirstDayInCalendarWeekAhead();
		fillCalendar();
	}

	public void onClickedOnEventMethod(MouseEvent mouseEvent){
		if(!mouseEvent.getButton().equals(MouseButton.PRIMARY) || mouseEvent.getClickCount() != 2){
            return;
        }
		Integer eventId = (Integer) ((Node) mouseEvent.getSource()).getProperties().get(EVENT_ID);

    	Event event = eventManager.getById(eventId);
    	try {
	        FXMLLoader fxmlLoader = new FXMLLoader();
	        fxmlLoader.setLocation(getClass().getResource("fxevent_edit.fxml"));
	        GridPane root = (GridPane) fxmlLoader.load();
	        for(Node node: root.getChildren()){
	        	if(EventEditController.datePickerId.equals(node.getId())){
	        		((DatePicker) node).setValue(event.getDate());
	        	} else if(EventEditController.startTimeHoursId.equals(node.getId())){
	        		((TextField) node).setText(event.getBeginTime().format(DateTimeFormatter.ofPattern("hh")));
	        	} else if(EventEditController.startTimeMinutesId.equals(node.getId())){
	        		((TextField) node).setText(event.getBeginTime().format(DateTimeFormatter.ofPattern("mm")));
	        	} else if(EventEditController.endTimeHoursId.equals(node.getId())){
	        		((TextField) node).setText(event.getEndTime().format(DateTimeFormatter.ofPattern("hh")));
	        	} else if(EventEditController.endTimeMinutesId.equals(node.getId())){
	        		((TextField) node).setText(event.getEndTime().format(DateTimeFormatter.ofPattern("mm")));
	        	} else if(EventEditController.eventNameId.equals(node.getId())){
	        		((TextField) node).setText(event.getName());
	        	} else if(EventEditController.eventDescriptionId.equals(node.getId())){
	        		((TextArea) node).setText(event.getDescription());
	        	}
	        }
	        Scene scene = new Scene(root,200, 200);
	        Stage stage = new Stage();
	        stage.setTitle("Edit event");
	        stage.setScene(scene);
	        stage.initModality(Modality.APPLICATION_MODAL);
	        stage.show();
	        ComponentsManager.getInstance().getEventEditController().setCurrentlyEditedEventId(eventId);
	    } catch (IOException e) {
	        e.printStackTrace();
	    }
	}

    public void onClickedOnDayMethod(MouseEvent mouseEvent) {
        if(!mouseEvent.getButton().equals(MouseButton.PRIMARY) || mouseEvent.getClickCount() != 2){
            return;
        }
        Label labelClickedOn = (Label) mouseEvent.getSource();
        Long xd = (Long) labelClickedOn.getProperties().get(CALENDAR_CARD_DAY);
        handleAddEventForDateAction(LocalDate.ofEpochDay(xd));
    }

	private void calculatefirstDayInCalendar(LocalDate date) {
		this.firstDayInCalendar = LocalDate.ofEpochDay(date.toEpochDay());
		while(!this.firstDayInCalendar.getDayOfWeek().equals(DayOfWeek.MONDAY)){
			this.firstDayInCalendar = firstDayInCalendar.minusDays(1);
		}
	}

	private void fillCalendar() {
		LocalDate[][] calendarCard = get28nextDaysFromDay(this.firstDayInCalendar);

		for(int week = 0; week < 4; week++){
			setTextForWeekLabels(week, calendarCard[week][0]);
			for(int day = 0; day < 7; day++){
				LocalDate date = calendarCard[week][day];

				List<Event> eventsForDay = eventManager.getSortedEventsForDay(date);
				VBox calendarCardDay = getVBoxInstanceOnWeekAndDay(week, day);

				setTitleForCalendarCardDay(date, calendarCardDay);
				setEventListForCalendarCardDay(eventsForDay, calendarCardDay);
			}
		}
	}

	private void setTextForWeekLabels(int week, LocalDate date) {
		Label[] weekLabels = getLabelInstancesForWeek(week);
		WeekFields weekFields = WeekFields.of(Locale.getDefault());
		Integer weekNr = date.get(weekFields.weekOfWeekBasedYear());
		Integer yearNr = date.getYear();
		String text = "W" + weekNr + "\n" + yearNr;
		weekLabels[0].setText(text);
		//weekLabels[0].setWrapText(true);
		weekLabels[1].setText(text);
		//weekLabels[1].setWrapText(true);
	}

	private Label[] getLabelInstancesForWeek(int week) {
		week++; //zmiena konwencji nadawania id dla pól w scenebuilderze na indeksowanie tablic
		try {
			Label leftLabel = (Label) this.getClass().getDeclaredField("week"+week+"L").get(this);
			Label rightLabel = (Label) this.getClass().getDeclaredField("week"+week+"P").get(this);
			return new Label[]{leftLabel, rightLabel};
		} catch (IllegalArgumentException | IllegalAccessException | NoSuchFieldException | SecurityException e) {
			e.printStackTrace();
		}

		throw new IllegalStateException();
	}

	private void setEventListForCalendarCardDay(List<Event> eventsForDay, VBox calendarCardDay) {
		VBox eventsContainer = (VBox) calendarCardDay.getChildren().get(1);
		eventsContainer.getChildren().clear();
		for(Event event : eventsForDay){
			Label eventDisplay = new Label();
			eventDisplay.setText(getTextForEvent(event));
			eventDisplay.getStyleClass().clear();
			eventDisplay.getStyleClass().add("dayCardSingleEventLabel");
			eventDisplay.getProperties().put(EVENT_ID, event.getId());
			eventDisplay.setOnMouseClicked(e -> onClickedOnEventMethod(e));
			eventsContainer.getChildren().add(eventDisplay);
		}
	}

	private String getTextForEvent(Event event){
		DateTimeFormatter dtf = DateTimeFormatter.ofPattern("hh:mm");
		String beginTime = dtf.format(event.getBeginTime());
		String endTime = dtf.format(event.getEndTime());
		return beginTime + "-" + endTime + " " + event.getName();
	}

	private Label setTitleForCalendarCardDay(LocalDate date, VBox calendarCardDay) {
		Label titleLabel = (Label) calendarCardDay.getChildren().get(0);
		titleLabel.setText(getCalendarCardTitleFromDate(date));
		titleLabel.getProperties().put(CALENDAR_CARD_DAY, date.toEpochDay());
		return titleLabel;
	}

	private VBox getVBoxInstanceOnWeekAndDay(int week, int day) {
		week++; day++; //zmiena konwencji nadawania id dla pól w scenebuilderze na indeksowanie tablic
		try {
			Field field = this.getClass().getDeclaredField("vbox"+week+""+day);
			VBox toReturn = (VBox) field.get(this);
			return toReturn;
		} catch (NoSuchFieldException | SecurityException e) {
			e.printStackTrace();
		} catch (IllegalArgumentException | IllegalAccessException e){
			e.printStackTrace();
		}
		throw new IllegalStateException();
	}

	private String getCalendarCardTitleFromDate(LocalDate date) {
		DateTimeFormatter dtf =  DateTimeFormatter.ofPattern("MMMM dd");
		return date.format(dtf);
	}

	private LocalDate[][] get28nextDaysFromDay(LocalDate firstDay) {
		LocalDate[][] toReturn = new LocalDate[4][7];
		for(int week = 0; week < 4; week++){
			for(int day = 0; day < 7; day++){
				toReturn[week][day] = LocalDate.ofEpochDay(firstDay.toEpochDay());
				firstDay = firstDay.plusDays(1);
			}
		}
		return toReturn;
	}

	private void handleAddEventForDateAction(LocalDate date) {
		try {
	        FXMLLoader fxmlLoader = new FXMLLoader();
	        fxmlLoader.setLocation(getClass().getResource("fxevent_edit.fxml"));
	        GridPane root = (GridPane) fxmlLoader.load();
	        root.getProperties().put(EVENT_ID, null);
	        for(Node node: root.getChildren()){
	        	if(EventEditController.datePickerId.equals(node.getId())){
	        		((DatePicker) node).setValue(date);
	        	}
	        }
	        Scene scene = new Scene(root,200, 200);
	        Stage stage = new Stage();
	        stage.setTitle("Add/Edit event");
	        stage.setScene(scene);
	        stage.initModality(Modality.APPLICATION_MODAL);
	        stage.show();
	    } catch (IOException e) {
	        e.printStackTrace();
	    }
	}

	private void moveFirstDayInCalendarWeekBackwards() {
		this.firstDayInCalendar = firstDayInCalendar.minusDays(7);
	}

	private void moveFirstDayInCalendarWeekAhead() {
		this.firstDayInCalendar = firstDayInCalendar.plusDays(7);
	}

	public void widthChanged(Number d2) {
		Double width = (Double) d2;
		String style = "-fx-font-size:" + String.valueOf(Math.floor(width / 50)) + ";";
		week1L.setStyle(style);
		week1P.setStyle(style);
		week2L.setStyle(style);
		week2P.setStyle(style);
		week3L.setStyle(style);
		week3P.setStyle(style);
		week4L.setStyle(style);
		week4P.setStyle(style);
		prevU.setStyle(style);
		prevD.setStyle(style);
		nextU.setStyle(style);
		nextD.setStyle(style);
	}

	public void heightChanged(Number d2) {
		// TODO Auto-generated method stub

		//return null;
	}
}
