<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Preventivo.aspx.cs" Inherits="BE_U3_W3_D4_Esercitazione_Auto._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div>
            <h1>Calcolo preventivo</h1>
            <div>
                <label for="ddlAuto">Seleziona un'auto:</label>
                <asp:DropDownList ID="ddlAuto" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAuto_Selezionata">
                </asp:DropDownList>

                <br />

                <asp:Image ID="imgAuto" runat="server" Visible="false" />

                <br />

                <label for="checkOptional">Seleziona gli optional:</label>
                <asp:CheckBoxList ID="checkOptional" runat="server">
                </asp:CheckBoxList>

                <br />

                <label for="ddlGaranzia">Seleziona il numero di anni di garanzia:</label>
                <asp:DropDownList ID="ddlGaranzia" runat="server">
                    <asp:ListItem Text="1 anno" Value="1" />
                    <asp:ListItem Text="2 anni" Value="2" />
                    <asp:ListItem Text="3 anni" Value="3" />
                </asp:DropDownList>
            </div>

            <br />

            <asp:Button ID="btnCalcola" runat="server" Text="Calcola Preventivo" OnClick="btnCalcola_Click" />

            <br />

            <div>
                <strong>Prezzo di base:</strong> <asp:Label ID="lblPrezzoBase" runat="server" Text="0.00" /><br />
                <strong>Totale Optional:</strong> <asp:Label ID="lblTotaleOptional" runat="server" Text="0.00" /><br />
                <strong>Totale Garanzia:</strong> <asp:Label ID="lblTotaleGaranzia" runat="server" Text="0.00" /><br />
                <strong>Totale Complessivo:</strong> <asp:Label ID="lblTotaleComplessivo" runat="server" Text="0.00" />
            </div>
        </div>
    </main>

</asp:Content>
