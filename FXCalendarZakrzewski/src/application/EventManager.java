package application;

import java.time.LocalDate;
import java.time.LocalTime;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;


public class EventManager {
	private static EventManager instance = null;
	private List<Event> events = new ArrayList<>();
	private AtomicInteger nextId = new AtomicInteger(0);

	static {
		EventManager eventManager = EventManager.getInstance();
		LocalDate today = LocalDate.now();
		LocalDate tomorrow = LocalDate.now().plus(1, ChronoUnit.DAYS);
		eventManager.insert(new Event(null, today, "name1", "desc1", LocalTime.of(1,2), LocalTime.of(3, 4)));
		eventManager.insert(new Event(null, today, "name2", "desc2", LocalTime.of(5,6), LocalTime.of(7, 8)));
		eventManager.insert(new Event(null, today, "name3", "desc3", LocalTime.of(9,10), LocalTime.of(11, 12)));
		eventManager.insert(new Event(null, tomorrow, "name4", "desc4", LocalTime.of(1,2), LocalTime.of(3, 4)));
		eventManager.insert(new Event(null, tomorrow, "name5", "desc5", LocalTime.of(5,6), LocalTime.of(7, 8)));
	}

	public static EventManager getInstance(){
		if(instance == null){
			instance = new EventManager();
		}
		return instance;
	}

	public void insert(Event eventToAdd){
		eventToAdd.setId(nextId.incrementAndGet());
		events.add(eventToAdd);
	}

	public List<Event> getSortedEventsForDay(LocalDate date){
		return events.stream()
				.filter(e -> e.getDate().isEqual(date))
				.sorted((e1, e2) -> e1.getBeginTime().compareTo(e2.getBeginTime()))
				.collect(Collectors.toList());
	}

	public void upsert(Event event) {
		if(event.getId() == null){
			event.setId(nextId.incrementAndGet());
		} else {
			events.remove(event);
		}
		events.add(event);
	}

	public Event getById(Integer eventId) {
		Optional<Event> event = events.stream().filter(ev -> ev.getId().equals(eventId)).findAny();
		if(event.isPresent()){
			return event.get();
		}
		return null;
	}

	public void deleteById(Integer eventId) {
		Event toDelete = null;
		for(Event e : events){
			if(e.getId().equals(eventId)){
				toDelete = e;
				break;
			}
		}
		events.remove(toDelete);
	}
}
