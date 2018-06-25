$(document).on('keypress', '.number', function (event) {

    if (event.keyCode == 46 || event.keyCode == 8) {
        // let it happen, don't do anything
    } else if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }

});
$(document).on('keypress', '.decimal', function (event) {

    if (event.keyCode == 46 || event.keyCode == 8) {
        // let it happen, don't do anything
    } else if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }

});