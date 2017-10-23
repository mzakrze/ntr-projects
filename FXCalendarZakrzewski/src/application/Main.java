package application;

import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.stage.Stage;
import javafx.scene.Scene;
import javafx.scene.layout.GridPane;

public class Main extends Application {
	@Override
	public void start(Stage primaryStage) {
		try {
			FXMLLoader loader = new FXMLLoader();
            loader.setLocation(Main.class.getResource("fxcalendar.fxml"));
            GridPane root = (GridPane) loader.load();

            Scene scene = new Scene(root, 400, 400);
            scene.getStylesheets().add(getClass().getResource("fxscene.css").toExternalForm());

            primaryStage.setScene(scene);
            primaryStage.setTitle("FXCalendar");
            primaryStage.show();
            primaryStage.setMinHeight(400);
            primaryStage.setMaxHeight(1600);
            primaryStage.setMinWidth(600);
            primaryStage.setMaxWidth(1800);
            CalendarController cc = ComponentsManager.getInstance().getCalendarController();
            primaryStage.widthProperty().addListener((e, d1, d2) -> cc.widthChanged(d2));
            primaryStage.heightProperty().addListener((e, d1, d2) -> cc.heightChanged(d2));
		} catch(Exception e) {
			e.printStackTrace();
		}
	}

	public static void main(String[] args) {
		launch(args);
	}
}
