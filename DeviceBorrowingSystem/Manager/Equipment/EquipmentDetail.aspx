<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EquipmentDetail.aspx.cs" Inherits="Manager_EquipmentDetail" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="DevExpress.Web.v14.1, Version=14.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxClasses" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title>Detail/Edit</title>
    <style type="text/css">
        .pageControl {
            width: 500px;
            height: 800px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div align="center">
            <div>
                <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" TabPosition="Top" Theme="Moderno">
                    <TabPages>
                        <dx:TabPage Text="Detail Info">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl1" runat="server">
                                    <iframe id="if_detail" runat="server" class="pageControl"></iframe>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                        <dx:TabPage Text="Calibration">
                            <ContentCollection>
                                <dx:ContentControl ID="ContentControl2" runat="server">
                                    <iframe id="if_cali" runat="server" class="pageControl"></iframe>
                                </dx:ContentControl>
                            </ContentCollection>
                        </dx:TabPage>
                    </TabPages>
                </dx:ASPxPageControl>
            </div>

        </div>
    </form>
</body>
</html>
