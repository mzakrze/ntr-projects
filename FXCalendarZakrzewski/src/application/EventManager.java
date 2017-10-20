package application;

import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.time.format.DateTimeFormatterBuilder;
import java.time.temporal.ChronoField;
import java.time.temporal.ChronoUnit;
import java.time.temporal.TemporalUnit;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Collection;
import java.util.Date;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;

import com.sun.xml.internal.bind.v2.runtime.unmarshaller.XsiNilLoader.Array;
import com.sun.xml.internal.ws.policy.privateutil.PolicyUtils.Collections;

import javafx.scene.input.MouseEvent;

public class EventManager {
	private static EventManager instance = null;
	private List<Event> events = new ArrayList<>();
	private AtomicInteger nextId = new AtomicInteger(0);

	static {
		EventManager eventManager = EventManager.getInstance();
		LocalDate today = LocalDate.now();
		LocalDate tomorrow = LocalDate.now().plus(1, ChronoUnit.DAYS);
		eventManager.insert(new Event(1, today, "name1", "desc1", LocalTime.of(1,2), LocalTime.of(3, 4)));
		eventManager.insert(new Event(2, today, "name2", "desc2", LocalTime.of(5,6), LocalTime.of(7, 8)));
		eventManager.insert(new Event(3, today, "name3", "desc3", LocalTime.of(9,10), LocalTime.of(11, 12)));
		eventManager.insert(new Event(4, tomorrow, "name4", "desc4", LocalTime.of(1,2), LocalTime.of(3, 4)));
		eventManager.insert(new Event(5, tomorrow, "name5", "desc5", LocalTime.of(5,6), LocalTime.of(7, 8)));

		DateTimeFormatter sdf = new DateTimeFormatterBuilder().toFormatter();//("yyyyMMdd");
		today.format(sdf);
	}

	public static EventManager getInstance(){
		if(instance == null){
			instance = new EventManager();
		}
		return instance;
	}

	public void insert(Event eventToAdd){
		boolean isIdUNotUnique = events.stream().anyMatch(e -> e.getId().equals(eventToAdd.getId()));
		if(isIdUNotUnique){
			throw new IllegalStateException(); // TODO napisać własny wyjątek
		}

		if(eventToAdd.getDate() == null || eventToAdd.getName() == null){ //TODO
			throw new IllegalStateException(); // TODO
		}

		if(!eventToAdd.getBeginTime().isBefore(eventToAdd.getEndTime())){
			throw new IllegalStateException(); // TODO
		}
		events.add(eventToAdd);
	}

	public List<Event> getSortedEventsForDay(LocalDate date){
		SimpleDateFormat sdf = new SimpleDateFormat("yyyyMMdd"); // TODO wyleczyć raka
		DateTimeFormatter dtf = new DateTimeFormatterBuilder().appendValue(ChronoField.DAY_OF_MONTH).appendValue(ChronoField.MONTH_OF_YEAR).appendValue(ChronoField.YEAR).toFormatter();
		return events.stream()
				//.filter(e -> sdf.format(e.getDate()).equals(sdf.format(date)))
				.filter(e -> e.getDate().format(dtf).equals(date.format(dtf)))
				.sorted((e1, e2) -> e1.getBeginTime().compareTo(e2.getBeginTime()))
				.collect(Collectors.toList());
	}

	public void upsert(Event event) {
		if(event.getId() == null){
			event.setId(nextId.incrementAndGet());
		} else {
			Integer index = events.indexOf(event);
			events.remove(index);
		}
		events.add(event);
	}
}
