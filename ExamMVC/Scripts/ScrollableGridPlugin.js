/*!
* This plug-in is developed for ASP.Net GridView control to make the GridView scrollable with Fixed headers.
*/
(function ($) {
    //1、定義一個jQuery例項方法，也是我們呼叫這個外掛的入口
    $.fn.Scrollable = function (options) {
    var defaults = {
    ScrollHeight: 300,
    Width: 0
    };

    //2、擴充套件集合，如果外部沒有傳入ScrollHeight的值，就預設使用300這個值，如果傳入，就用傳入的ScrollHeight值
    var options = $.extend(defaults, options); 
    return this.each(function () {
        //3、獲取當前gridview控制元件的物件
        var grid = $(this).get(0);
        //4、獲取gridview的寬度
        var gridWidth = grid.offsetWidth;
        var headerCellWidths = new Array();
        //5、建立了一個儲存表頭各個標題列的寬度的陣列
        for (var i = 0; i < grid.getElementsByTagName("TH").length; i  ) {
            headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
        }

        //6、在文件中gridview的同級位置最後加一個div元素
        grid.parentNode.appendChild(document.createElement("div"));
        //7、gridview的父節點，是個div
        var parentDiv = grid.parentNode;
        //8、在dom中建立一個table元素
        var table = document.createElement("table");
        //9、把gridview的所有屬性加到新建立的table元素上
        for (i = 0; i < grid.attributes.length; i  ) {
            if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
                table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
            }
        }

        //10、將樣式也加到table元素上
        table.style.cssText = grid.style.cssText;
        //11、為table元素設定與gridview同樣的寬
        table.style.width = gridWidth+"px";
        //12、為table元素加一個tbody元素
        table.appendChild(document.createElement("tbody"));
        //13、把gridview中的第一行資料加到tbody元素中，即table裡現在存著一行gridview的標題元素，同時在gridview裡刪除了標題這一行的元素
        table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
        
        //14、取得表格標題列的集合
        var cells = table.getElementsByTagName("TH");
        //15、gridview中第一行資料列的集合
        var gridRow = grid.getElementsByTagName("TR")[0];
        for (var i = 0; i < cells.length; i  ) {
            var width;
            //16、如果標題格的寬度大於資料列的寬度
            if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
                width = headerCellWidths[i];
            }
            //17、如果標題格的寬度小於資料列的寬度
            else {
                width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
            }

            cells[i].style.width = parseInt(width - 3)+"px";
            //18、將每個標題列和資料列的寬度均調整為取其中更寬的一個，-3是出於對錶格的樣式進行微調考慮，不是必須
            gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 3)+"px";
        }

        //19、刪除gridview控制元件（注意只是從文件流中刪除，實際還在記憶體當中，注意27條），現在的情況是table裡只有一個表頭
        parentDiv.removeChild(grid);
        //20、在文件中再建立一個div元素
        var dummyHeader = document.createElement("div");
        //21、把表頭table加入其中
        dummyHeader.appendChild(table);
        //22、把這個div插入到原來gridview的位置裡
        parentDiv.appendChild(dummyHeader);

        //23、如果我們最初傳入了一個想要的表格寬度值，就將gridWidth賦上這個值
        if (options.Width > 0) {
            gridWidth = options.Width;
        }

        //24、再建立一個div
        var scrollableDiv = document.createElement("div");

        //25、在原基礎上 17是因為這是一個具有滑動條的table，當出現滑動條的時候，會在寬度上多出一個條的寬度，為了使資料列與標題列保持一致，要把這個寬度算進行，17這個值也不是必須，這個可以試驗著來。
        gridWidth = parseInt(gridWidth)+17;
        //26、給具有滾動條的div加上樣式，height就是我們想讓它在多大的長度時出現滾動條
        scrollableDiv.style.cssText = "overflow:auto;height:" + options.ScrollHeight + "px;width:" + gridWidth + "px";
        //27、將gridview（目前只存在資料存在資料列）加到這個帶有滾動條的div中，這裡是從記憶體中將grid取出
        scrollableDiv.appendChild(grid);
        //28、將帶有滾動條的div加到table的下面
        parentDiv.appendChild(scrollableDiv);
    });
};
})(jQuery);