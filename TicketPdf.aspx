<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TicketPdf.aspx.cs" Inherits="TicketPdf" %>

<%@ Register Assembly="E-Utilities" Namespace="E_Utilities" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Impresion de Documentos</title>
</head>
<body>
    <form id="form1" runat="server" style="width:100%; height:100%; min-height:100%; margin:0; top:0; left:0; padding:0;">        
        <cc1:ShowPdf ID="ShowPdf1" runat="server" Width="100%" Height="1000px" BorderStyle="Inset" BorderWidth="2px" />    
    </form>
</body>
</html>
