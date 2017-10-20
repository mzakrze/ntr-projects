package application;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Arrays;
import java.util.Date;
import java.util.List;

public class DateManager {
	private DateFormat dateFormat = new SimpleDateFormat("MM/dd");


	public DateManager(){

	}

	public Date getCurrentDate(){
		Date date = new Date(); // domyślnie tworzy obecną datę
		return date;
	}

}
