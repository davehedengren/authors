<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodreadsAPITutorialASPNet.aspx.cs" Inherits="JWinston.ExternalAPIs.GoodreadsAPITutorialASPNet" %>
<script src="scripts/jquery-1.8.0.min.js" type="text/javascript"></script>
<script src="scripts/goodreadsGetBookInfoJavascript.js" type="text/javascript"></script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h2>Get Book Information</h2>
    Author: <input type="text" id="authorTextbox" value="Patrick Rothfuss" /> <br />
    Title: <input type="text" id="titleTextbox" value="The Name of the Wind" /> <br />
    <input type="button" value="Get Book Information" id="getButton" onclick='getBookInformation()' />
    <br /><br />
    <div id="DataContainer" ></div>
    </form>
</body>
</html>
