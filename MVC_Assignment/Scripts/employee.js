



$(document).ready(function () {

        $("#ddlLocations").prop("disabled", true);

        $('input[name=searchby]').click(function () {

            if ($(this).attr('id') == 'radioBtnLoc') {
                $("#ddlLocations").prop("disabled", false).show();
                $('#searchtxt').attr('value', $('option:selected').text());

            }

            else {

                $("#ddlLocations").prop("disabled", true);
                $('#searchtxt').attr('value', "");
            }

        })

        $("#ddlLocations").change(function () {
            $('#searchtxt').attr('value', $('option:selected').text());

        })
    
});