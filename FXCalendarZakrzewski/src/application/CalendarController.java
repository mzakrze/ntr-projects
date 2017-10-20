package application;

import java.io.IOException;
import java.lang.reflect.Field;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeFormatterBuilder;
import java.time.temporal.WeekFields;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import javax.swing.JTextField;

import javafx.beans.property.IntegerProperty;
import javafx.beans.property.SimpleIntegerProperty;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.VBox;
import javafx.stage.Modality;
import javafx.stage.Stage;
import javafx.scene.Node;
import javafx.scene.Scene;
import javafx.scene.control.DatePicker;
import javafx.scene.control.Label;
import javafx.scene.control.TableView;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javafx.scene.input.MouseButton;
import javafx.scene.input.MouseEvent;

public class CalendarController {
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

	private LocalDate today = LocalDate.now();
	private LocalDate firstDayInCalendar;
	private EventManager eventManager = EventManager.getInstance();

	private Label[][] calendarCards = new Label[4][7];
	private List<Label>[][] calendarCardsEvents = new ArrayList[4][7];

	private final static String CALENDAR_CARD_DAY = "CALENDAR_CARD_DAY";
	private final static String CALENDAR_CARD_DAY_EVENT_ID = "CALENDAR_CARD_DAY_EVENT_ID";
	private static final String EVENT_ID = "EVENT_ID";

	public CalendarController(){

	}

	@FXML
    public void initialize() {
		calculatefirstDayInCalendar(today);

		fillCalendar();
    }

	private void fillCalendar() {
		LocalDate[][] calendarCard = get28nextDaysFromDay(this.firstDayInCalendar);

		for(int week = 0; week < 4; week++){
			Label[] weekLabels = getLabelInstancesForWeek(week);
			String text = getTextForWeekLabel(calendarCard[week][0]);
			weekLabels[0].setText(text);
			weekLabels[1].setText(text);

			for(int day = 0; day < 7; day++){
				LocalDate date = calendarCard[week][day];
				String calendarCardDayTitle = getCalendarCardTitleFromDate(date);
				List<Event> eventsForDay = eventManager.getSortedEventsForDay(date);

				VBox calendarCardDay = getVBoxInstanceOnWeekAndDay(week, day);



				this.calendarCards[week][day] = setTitleForCalendarCardDay(date, calendarCardDayTitle, calendarCardDay);
				this.calendarCardsEvents[week][day] = setEventListForCalendarCardDay(eventsForDay, calendarCardDay);
			}
		}
	}

	private String getTextForWeekLabel(LocalDate date) {
		WeekFields weekFields = WeekFields.of(Locale.getDefault());
		Integer weekNr = date.get(weekFields.weekOfWeekBasedYear());
		Integer yearNr = date.getYear();
		return "W" + weekNr + " " + yearNr;
	}

	private Label[] getLabelInstancesForWeek(int week) {
		week++;
		try {
			Label leftLabel = (Label) this.getClass().getDeclaredField("week"+week+"L").get(this);
			Label rightLabel = (Label) this.getClass().getDeclaredField("week"+week+"P").get(this);
			return new Label[]{leftLabel, rightLabel};
		} catch (IllegalArgumentException | IllegalAccessException | NoSuchFieldException | SecurityException e) {
			e.printStackTrace();
		}

		return null;
	}

	private List<Label> setEventListForCalendarCardDay(List<Event> eventsForDay, VBox calendarCardDay) {
		List<Label> labels = new ArrayList<>();
		VBox eventsContainer = (VBox) calendarCardDay.getChildren().get(1);
		eventsContainer.getChildren().clear();
		for(Event event : eventsForDay){
			Label eventDisplay = new Label();
			eventDisplay.setText(getTextForEvent(event));
			eventDisplay.getStyleClass().clear();
			eventDisplay.getStyleClass().add("dayCardSingleEventLabel");
			eventsContainer.getChildren().add(eventDisplay);
			labels.add(eventDisplay);
		}
		return labels;
	}

	private String getTextForEvent(Event event){
		DateTimeFormatter dtf = DateTimeFormatter.ofPattern("hh:mm");
		String beginTime = dtf.format(event.getBeginTime());
		String endTime = dtf.format(event.getEndTime());
		String trimmedName = event.getName().substring(0, 5);
		return beginTime + "-" + endTime + " " + trimmedName;
	}

	private Label setTitleForCalendarCardDay(LocalDate date, String calendarCardDayTitle, VBox calendarCardDay) {
		Label titleLabel = (Label) calendarCardDay.getChildren().get(0);
		titleLabel.setText(calendarCardDayTitle);
		titleLabel.getProperties().put(CALENDAR_CARD_DAY, date.toEpochDay());
		return titleLabel;
	}

	private VBox getVBoxInstanceOnWeekAndDay(int week, int day) {
		week++; day++; // zmiana konwencji
		// TODO - uwspólnieć konwencje
		try {
			Field field = this.getClass().getDeclaredField("vbox"+week+""+day);
			VBox toReturn = (VBox) field.get(this);
			return toReturn;
		} catch (NoSuchFieldException | SecurityException e) {
			e.printStackTrace();
		} catch (IllegalArgumentException | IllegalAccessException e){
			e.printStackTrace();
		}
		return null;
	}

	private String getCalendarCardTitleFromDate(LocalDate date) {
		//return new SimpleDateFormat("MMMMM dd").format(date);
		DateTimeFormatter dtf = new DateTimeFormatterBuilder().toFormatter();
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

	private void calculatefirstDayInCalendar(LocalDate date) {
		this.firstDayInCalendar = LocalDate.ofEpochDay(date.toEpochDay());
		System.out.print("firstDayInCalendar:" + this.firstDayInCalendar);
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
		if(mouseEvent.getButton().equals(MouseButton.PRIMARY)){
            if(mouseEvent.getClickCount() == 2){

            }
        }
	}

    public void onClickedOnDayMethod(MouseEvent mouseEvent) {
        if(mouseEvent.getButton().equals(MouseButton.PRIMARY)){
            if(mouseEvent.getClickCount() == 2){
            	Label labelClickedOn = (Label) mouseEvent.getSource();
                Long xd = (Long) labelClickedOn.getProperties().get(CALENDAR_CARD_DAY);
                handleAddEventForDateAction(LocalDate.ofEpochDay(xd));
            }
        }
    }

	private void handleAddEventForDateAction(LocalDate date) {
		try {
	        FXMLLoader fxmlLoader = new FXMLLoader();
	        fxmlLoader.setLocation(getClass().getResource("fxevent_edit.fxml"));
	        GridPane root = (GridPane) fxmlLoader.load();
	        root.getProperties().put(EVENT_ID, null);
	        for(Node node: root.getChildren()){
	        	if(node instanceof DatePicker){
	        		((DatePicker) node).setValue(date);
	        	}
	        }
	        Scene scene = new Scene(root,200, 200);
	        Stage stage = new Stage();
	        stage.setTitle("New Window");
	        stage.setScene(scene);
	        stage.initModality(Modality.APPLICATION_MODAL);
	        stage.show();


	    } catch (IOException e) {
	        e.printStackTrace();
	    }
	}

	private void onEventEditSubmitButtonClicked(MouseEvent event){
		((Node) event.getSource()).getParent();
	}

	private void displayAddEventForWeekDay(int week, int day) {
		System.out.println("week:" + week + ", day:" + day);
		// TODO Auto-generated method stub

	}

	private void moveFirstDayInCalendarWeekBackwards() {
		this.firstDayInCalendar = firstDayInCalendar.minusDays(7);
	}

	private void moveFirstDayInCalendarWeekAhead() {
		this.firstDayInCalendar = firstDayInCalendar.plusDays(7);
	}
}
