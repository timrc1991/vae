// main form submission function for bigram parsing
$('form button').click(function () {
    // setting ajax POST info for text
    var data = { text: $('form textarea').val() };
    var url = '/Home/ParseBigrams';
    var type = 'application/x-www-form-urlencoded; charset=UTF-8';
    var process = true;

    // if user chose a file, change ajax POST info
    if ($('form input[value="file"]').is(':checked')) {
        url = '/Home/ParseBigramsFromFile';
        data = new FormData($('form')[0]);
        type = false;
        process = false;
    }

    // make call to controller method to receive bigrams
    $.ajax({
        url: url,
        type: 'POST',
        data: data,
        contentType: type,
        processData: process,
        success: function (result) {
            if (result.success === true) {
                $('.error').html('');

                $('.chart .results').empty(); // remove existing data in chart

                var max = -1;
                $.each(result.content, function (key, val) {
                    if (key === 0) {
                        max = val.value; // 1st item in list has max value
                    }

                    // create new column to add to histogram
                    var left = 'left:' + (key * 20) + 'px;';
                    var height = 'height:' + ((val.value / max) * 400) + 'px;';
                    var label = '<span class="label">' + val.key + '</span>';
                    var score = '<span class="score">' + val.value + '</span>';
                    $('.chart .results').append('<div class="column" style="' + left + height + '%">' + label + score + '</div>');
                });

                $('.chart .max').html(max); // set the upper value of the chart to the max value found
            }
            else {
                $('.error').html(result.message); // display error message from call
            }
        }
    });
});

// this function tracks changes to user input fields and submits the bigram form on-change
$('form input[type="file"], form textarea').on('change input', function () {
    $('form button').click();
});

// this method hides/shows the text or file inputs based on a radio selection
$('form input[type="radio"]').click(function () {
    if (this.value === 'file') {
        $('form textarea').hide();
        $('form input[type="file"]').show();
    }
    else {
        $('form textarea').show();
        $('form input[type="file"]').hide();
    }
});