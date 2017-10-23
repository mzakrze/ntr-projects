package application;

import java.time.LocalDate;
import java.time.LocalTime;

/**
 * POJO class representing event
 * @author mariusz
 *
 */
public class Event {
	private Integer id;
	private LocalDate date;
	private String name;
	private String description;
	private LocalTime beginTime;
	private LocalTime endTime;

	public Event(Integer id, LocalDate date, String name, String description, LocalTime beginTime, LocalTime endTime) {
		super();
		this.id = id;
		this.date = date;
		this.name = name;
		this.description = description;
		this.beginTime = beginTime;
		this.endTime = endTime;
	}

	public Integer getId() {
		return id;
	}
	public void setId(Integer id) {
		this.id = id;
	}
	public LocalDate getDate() {
		return date;
	}
	public void setDate(LocalDate date) {
		this.date = date;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public String getDescription() {
		return description;
	}
	public void setDescription(String description) {
		this.description = description;
	}
	public LocalTime getBeginTime() {
		return beginTime;
	}
	public void setBeginTime(LocalTime beginTime) {
		this.beginTime = beginTime;
	}
	public LocalTime getEndTime() {
		return endTime;
	}
	public void setEndTime(LocalTime endTime) {
		this.endTime = endTime;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((id == null) ? 0 : id.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Event other = (Event) obj;
		if (id == null) {
			if (other.id != null)
				return false;
		} else if (!id.equals(other.id))
			return false;
		return true;
	}

}
