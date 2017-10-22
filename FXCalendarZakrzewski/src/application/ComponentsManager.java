package application;

public class ComponentsManager {
	private static ComponentsManager INSTANCE = null;
	private EventEditController eventEditController;
	private CalendarController calendarController;

	public static ComponentsManager getInstance(){
		if(INSTANCE == null){
			INSTANCE = new ComponentsManager();
		}
		return INSTANCE;
	}
	private ComponentsManager(){

	}

	public void registerComponentAsEventEditController(EventEditController controller){
		this.eventEditController = controller;
	}

	public void registerComponentAsCalendarController(CalendarController controller){
		this.calendarController = controller;
	}

	public EventEditController getEventEditController() {
		return eventEditController;
	}
	public CalendarController getCalendarController() {
		return calendarController;
	}


}
