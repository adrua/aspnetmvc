$.jgrid.extend({
    getLastSelRow: function () {
        return this.attr("lastSelRow");
    },
    setLastSelRow: function (rowid) {
        this.attr("lastSelRow",  rowid);
        return rowid;
    },
    getCellValue: function (elem, ope) {

        if (ope == "FormElement") {
            return $("#" + elem).val();
        }

        var rowid = this.jqGrid('getLastSelRow');
        var qElem = "#" + rowid + "_" + elem;
        if (ope == "editable") {
            return $(qElem).val();
        };

        var value = this.jqGrid('getCell', rowid, elem);
        if (value && value.match(/\<input /)) {
            value = $(qElem).val();
        }
        return value;
    },

    setCellValue: function (elem, value, ope) {
        if (ope == "FormElement") {
            $("#" + elem).val(value);
            return value;
        }

        var rowid = this.jqGrid('getLastSelRow');
        var qElem = "#" + rowid + "_" + elem;
        if (ope == "editable") {
            $(qElem).val(value);
            return value;
        };

        var op = this.jqGrid('getCell', rowid, elem);
        if (op && op.match(/\<input /)) {
            $(qElem).val(value);
        }
        else {
            this.jqGrid('setCell', rowid, elem, value);
        }

        return value;
    },

    checkdatetime: function (value, colname) {
        var vals = value.split(' ');
        var dft = 'Y-m-d';
        var tft = 'H-i';
        var bDate = $.jgrid.checkDate(dft, vals[0]);
        var bTime = $.jgrid.checkTime(vals[1]);
        if (bDate && bTime) 
            return [true,""];
        else 
            return [false,colname+": "+$.jgrid.edit.msg.date+" - "+dft + " "+ tft];
    }

});
