function getBookInformation() {
    $.get('goodreadsHttpHandler.ashx'
                // Specify location of Author and Title to search for.
            , { bookAuthor: $('#authorTextbox').val(), bookTitle: $('#titleTextbox').val() }
            , function (data) {
                var titleStyle = "<p style=\"color:#666600;font-family:georgia,serif;\">";
                var spanStart = "<span style=\"color:black;\">";
                var spanEnd = "</span>";
                var reviewsDiv = "<div id=\"ReviewContainer\"><p style=\"cursor:pointer;color:#666600;font-family:georgia,serif;\">";
                var infoFromGoodreads = "<br/><br/><br/><p style=\"font-size:9px;\">Information provided by <a href=\"http://www.goodreads.com\">goodreads</a>.</p>";
                if (data.Status === "Status_OK") {
                    // Specify div to fill (change in else statement below also).
                    $('#DataContainer').html(
                "<table><tbody><tr>"
                + "<td style=\"width:100px;\">" + "<img src=" + data.Cover_Image + "></>" + "</td>"
                + "<td>" + titleStyle
                + "Author: " + spanStart + data.Author + spanEnd
                + "<br/>" + "Title: " + spanStart + data.Title + spanEnd
                + "<br/>" + "Average Rating: " + spanStart + data.Average_Rating + spanEnd
                + "<br/>" + "First Published: " + spanStart + data.Publication_Year + spanEnd
                + "<br/>" + "Publisher: " + spanStart + data.Publisher + spanEnd
                + "<br/>" + "ISBN: " + spanStart + data.ISBN
                + "</p></td></tr></tbody></table>"
                + titleStyle + "Description" + "<br/><br/>" + spanStart + data.Description + spanEnd + "</p>"
                + reviewsDiv + "Show reviews..." + "</p></div>"
                + infoFromGoodreads
                );
                } else if (data.Status === "Bad_XML") {
                    $('#DataContainer').html("Unexpected XML encountered." + infoFromGoodreads);
                } else {
                    $('#DataContainer').html("No book found." + infoFromGoodreads);
                }
                $("#ReviewContainer").click(function () { $('#ReviewContainer').html(data.Reviews); });
            }
        );
}