var appointment = {};

$('.clockpicker.start-time').clockpicker();
$('.clockpicker.end-time').clockpicker();

$('.day').dblclick(function () {
    appointment = { AppointmentDate: $(this).find('input.value-date').val() };
    $('#modal-title').text('Add new appointment');
    $('.btn-danger').css('display', 'none');
    $('#title').val('');
    $('#description').val('');
    $('.start-time input').val('12:00');
    $('.end-time input').val('13:00');
    $('.errors').text('');
    $('#myModal').modal('show');
});

$('.appointment').dblclick(function (event) {
    event.preventDefault();
    appointment.AppointmentId = $(this).find('input.value-appointmentid').val();
    $.get('/api/appointments/' + appointment.AppointmentId, function (data) {
        appointment = data;
        $('#modal-title').text('Edit appointment');
        $('.btn-danger').css('display', 'inline');
        $('#title').val(data.Title);
        $('#description').val(data.Description);
        $('.start-time input').val(data.StartTime.substring(0, 5));
        $('.end-time input').val(data.EndTime.substring(0, 5));
        $('.errors').text('');
        $('#myModal').modal('show');
    });
    return false;
});


$('#myModal .btn-primary').click(function () {
    appointment.Title = $('#title').val();
    appointment.Description = $('#description').val();
    appointment.StartTime = $('.start-time input').val();
    appointment.EndTime = $('.end-time input').val();

    if (appointment.AppointmentID) {
        $.ajax({
            type: 'PUT',
            dataType: 'json',
            contentType: 'application/json; charset=UTF-8',
            url: '/api/appointments/' + appointment.AppointmentID,
            data: JSON.stringify(appointment),
            success: function (data) {
                $('#myModal').modal('hide');
                location.reload();
            }
        });
    } else {
        $.ajax({
            type: 'POST',
            dataType: 'json',
            contentType: 'application/json; charset=UTF-8',
            url: '/api/appointments/',
            data: JSON.stringify(appointment),
            success: function (data) {
                $('#myModal').modal('hide');
                location.reload();
            }
        });
    }
});

$('#myModal .btn-danger').click(function () {
    $.ajax({
        type: 'DELETE',
        url: '/api/appointments/' + appointment.AppointmentID,
        success: function (data) {
            $('#myModal').modal('hide');
            location.reload();
        }
    });
});

$('.navigate').click(function () {
    if ($(this).text().indexOf('prev') >= 0)
        go(-7);
    else
        go(7);
});

function go(days) {
    var dateStr = window.location.search.substr(6);
    var date;
    if (dateStr.length === 10) {
        date = new Date(dateStr);
    } else {
        date = new Date();
    }
    date = addDays(date, days);
    window.location = '/?date=' + date.toISOString().substr(0, 10);
}

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}

