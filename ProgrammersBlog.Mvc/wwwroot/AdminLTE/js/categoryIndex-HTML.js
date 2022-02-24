$(document).ready(function () {
    $('#categoriesTable').DataTable({
        "order": [[4, "desc"]],
        dom:
            "<'row'<'col-sm-3'l><'col-sm-6 text-center'B><'col-sm-3'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",
        buttons: [
            {
                text: 'Ekle',
                className: 'btn btn-succes',
                /* Atr içinde verdiğimiz id ye sahiğ alttaki script kodunu çağrcaz */
                attr: {
                    id: "btnAdd"
                },
                action: function (e, dt, node, config) {

                }
            },
            {
                text: 'Yenile',
                className: 'btn btn-warning',
                action: function (e, dt, node, config) {
                    //jquery ajax fonksiyonu ile yenileme işlemini yapyoruz. Veri aldığımız için Get fonksiyonu. url kısmında hangi actiondan get yapıtığımızı belirtiyoruç
                    $.ajax({
                        type: 'GET',
                        url: '/Admin/Category/GetAllCategories',
                        //veriler get işlemi sonucu data json formatında geliyor.
                        contentType: "application/json",
                        //fonksiyon çalışmadan önce ne yapılacak.
                        beforeSend: function () {
                            $('#categoriesTable').hide();
                            $('.spinner-border').show();
                        },
                        //fonksiyon başarılıysa gelen veriyle yeni tablo içeriği oluşturuyoruz ve önceki tablo içeriğini bunla değiştiryoruz.
                        success: function (data) {
                            const categoryListDto = jQuery.parseJSON(data);
                            console.log(categoryListDto);
                            if (categoryListDto.ResultStatus === 0) {
                                let tableBody = "";
                                $.each(categoryListDto.Categories.$values,
                                    function (index, category) {
                                        tableBody += `
                                                <tr name="${category.Id}">
                                    <td>${category.Id}</td>
                                    <td>${category.Name}</td>
                                    <td>${category.Description}</td>
                                    <td>${category.IsActive ?"Evet" : "Hayır"}</td>
                                    <td>${category.IsDeleted ? "Evet" : "Hayır"}</td>
                                    <td>${category.Note}</td>
                                    <td>${convertToShortDate(category.CreatedDate)}</td>
                                    <td>${category.CreatedByName}</td>
                                    <td>${convertToShortDate(category.ModifiedDate)}</td>
                                    <td>${category.ModifiedByName}</td>
                                <td>
                                    <button class="btn btn-primary btn-sm btn-update" data-id="${category.Id}"><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-danger btn-sm btn-delete" data-id="${category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                                            </tr>`;
                                    });
                                $('#categoriesTable > tbody').replaceWith(tableBody);
                                $('.spinner-border').hide();
                                $('#categoriesTable').fadeIn(1400);
                            } else {
                                toastr.error(`${categoryListDto.Message}`, 'İşlem Başarısız!');
                            }
                        },
                        error: function (err) {
                            console.log(err);
                            $('.spinner-border').hide();
                            $('#categoriesTable').fadeIn(1000);
                            toastr.error(`${err.responseText}`, 'Hata!');
                        }

                    });
                }
            }
        ],
        language: {
            "emptyTable": "Tabloda herhangi bir veri mevcut değil",
            "info": "_TOTAL_ kayıttan _START_ - _END_ arasındaki kayıtlar gösteriliyor",
            "infoEmpty": "Kayıt yok",
            "infoFiltered": "(_MAX_ kayıt içerisinden bulunan)",
            "infoThousands": ".",
            "lengthMenu": "Sayfada _MENU_ kayıt göster",
            "loadingRecords": "Yükleniyor...",
            "processing": "İşleniyor...",
            "search": "Ara:",
            "zeroRecords": "Eşleşen kayıt bulunamadı",
            "paginate": {
                "first": "İlk",
                "last": "Son",
                "next": "Sonraki",
                "previous": "Önceki"
            },
            "aria": {
                "sortAscending": ": artan sütun sıralamasını aktifleştir",
                "sortDescending": ": azalan sütun sıralamasını aktifleştir"
            },
            "select": {
                "rows": {
                    "_": "%d kayıt seçildi",
                    "1": "1 kayıt seçildi"
                },
                "cells": {
                    "1": "1 hücre seçildi",
                    "_": "%d hücre seçildi"
                },
                "columns": {
                    "1": "1 sütun seçildi",
                    "_": "%d sütun seçildi"
                }
            },
            "autoFill": {
                "cancel": "İptal",
                "fillHorizontal": "Hücreleri yatay olarak doldur",
                "fillVertical": "Hücreleri dikey olarak doldur",
                "fill": "Bütün hücreleri <i>%d<\/i> ile doldur"
            },
            "buttons": {
                "collection": "Koleksiyon <span class=\"ui-button-icon-primary ui-icon ui-icon-triangle-1-s\"><\/span>",
                "colvis": "Sütun görünürlüğü",
                "colvisRestore": "Görünürlüğü eski haline getir",
                "copySuccess": {
                    "1": "1 satır panoya kopyalandı",
                    "_": "%ds satır panoya kopyalandı"
                },
                "copyTitle": "Panoya kopyala",
                "csv": "CSV",
                "excel": "Excel",
                "pageLength": {
                    "-1": "Bütün satırları göster",
                    "_": "%d satır göster"
                },
                "pdf": "PDF",
                "print": "Yazdır",
                "copy": "Kopyala",
                "copyKeys": "Tablodaki veriyi kopyalamak için CTRL veya u2318 + C tuşlarına basınız. İptal etmek için bu mesaja tıklayın veya escape tuşuna basın."
            },
            "searchBuilder": {
                "add": "Koşul Ekle",
                "button": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "condition": "Koşul",
                "conditions": {
                    "date": {
                        "after": "Sonra",
                        "before": "Önce",
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "number": {
                        "between": "Arasında",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "gt": "Büyüktür",
                        "gte": "Büyük eşittir",
                        "lt": "Küçüktür",
                        "lte": "Küçük eşittir",
                        "not": "Değildir",
                        "notBetween": "Dışında",
                        "notEmpty": "Dolu"
                    },
                    "string": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "endsWith": "İle biter",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "startsWith": "İle başlar"
                    },
                    "array": {
                        "contains": "İçerir",
                        "empty": "Boş",
                        "equals": "Eşittir",
                        "not": "Değildir",
                        "notEmpty": "Dolu",
                        "without": "Hariç"
                    }
                },
                "data": "Veri",
                "deleteTitle": "Filtreleme kuralını silin",
                "leftTitle": "Kriteri dışarı çıkart",
                "logicAnd": "ve",
                "logicOr": "veya",
                "rightTitle": "Kriteri içeri al",
                "title": {
                    "0": "Arama Oluşturucu",
                    "_": "Arama Oluşturucu (%d)"
                },
                "value": "Değer",
                "clearAll": "Filtreleri Temizle"
            },
            "searchPanes": {
                "clearMessage": "Hepsini Temizle",
                "collapse": {
                    "0": "Arama Bölmesi",
                    "_": "Arama Bölmesi (%d)"
                },
                "count": "{total}",
                "countFiltered": "{shown}\/{total}",
                "emptyPanes": "Arama Bölmesi yok",
                "loadMessage": "Arama Bölmeleri yükleniyor ...",
                "title": "Etkin filtreler - %d"
            },
            "thousands": ".",
            "datetime": {
                "amPm": [
                    "öö",
                    "ös"
                ],
                "hours": "Saat",
                "minutes": "Dakika",
                "next": "Sonraki",
                "previous": "Önceki",
                "seconds": "Saniye",
                "unknown": "Bilinmeyen",
                "weekdays": {
                    "6": "Paz",
                    "5": "Cmt",
                    "4": "Cum",
                    "3": "Per",
                    "2": "Çar",
                    "1": "Sal",
                    "0": "Pzt"
                },
                "months": {
                    "9": "Ekim",
                    "8": "Eylül",
                    "7": "Ağustos",
                    "6": "Temmuz",
                    "5": "Haziran",
                    "4": "Mayıs",
                    "3": "Nisan",
                    "2": "Mart",
                    "11": "Aralık",
                    "10": "Kasım",
                    "1": "Şubat",
                    "0": "Ocak"
                }
            },
            "decimal": ",",
            "editor": {
                "close": "Kapat",
                "create": {
                    "button": "Yeni",
                    "submit": "Kaydet",
                    "title": "Yeni kayıt oluştur"
                },
                "edit": {
                    "button": "Düzenle",
                    "submit": "Güncelle",
                    "title": "Kaydı düzenle"
                },
                "error": {
                    "system": "Bir sistem hatası oluştu (Ayrıntılı bilgi)"
                },
                "multi": {
                    "info": "Seçili kayıtlar bu alanda farklı değerler içeriyor. Seçili kayıtların hepsinde bu alana aynı değeri atamak için buraya tıklayın; aksi halde her kayıt bu alanda kendi değerini koruyacak.",
                    "noMulti": "Bu alan bir grup olarak değil ancak tekil olarak düzenlenebilir.",
                    "restore": "Değişiklikleri geri al",
                    "title": "Çoklu değer"
                },
                "remove": {
                    "button": "Sil",
                    "confirm": {
                        "_": "%d adet kaydı silmek istediğinize emin misiniz?",
                        "1": "Bu kaydı silmek istediğinizden emin misiniz?"
                    },
                    "submit": "Sil",
                    "title": "Kayıtları sil"
                }
            }
        }
    });


    /* Datatables end here */

    /* Ajax GET / Getting the _CategoryAddPartial as Modal Form strats from here */

    $(function () {
        // Category controllerındaki add actionu hangi viewı dönüyorsa onun url ini bize getiricek. Add actionu bize kategori ekleme modal'ı olan viewi dönüyor
        const url = '/Admin/Category/Add';
        //tanımladığımız modelPlaceHolder id' li divi değişkene atadık.
        const placeHolderDiv = $('#modalPlaceHolder');
        //DataTables içinde ki butonlardan birinde "attr:" içinde btnAdd id si tanımlamıştık. Burda onu seçerek o butona
        //tıklandığında fonksiyon çalışmasını istiyoruz.
        $('#btnAdd').click(function () {

            //$get ile üstte aldığımız url'e gidiyor ve onun içeriğini getiriyor. Funciton içindeki data'ya aktarıyor bu içeriği.
            $.get(url).done(function (data) {
                //en üstte tanımladığımız divin içeriğinin data olmasını sağlıyor.
                placeHolderDiv.html(data);
                //normalde en üstteki divi hidden yapmıştık. Burada içindeki modal class ı olan kısmı bul ve Modal olarak göster yani görünür yap diyoruz.
                placeHolderDiv.find(".modal").modal('show');
            });

            /* Ajax GET / Getting the _CategoryAddPartial as Modal Form ends here */

            /* Ajax POST / Posting the FormData as CategoryAddDto starts from here */

            //Bu kısımda  formdaki veriyi post edip dönen dataya göre kontrol ve tabloya ekleme işlemi yapıyoruz.

            //placeholder üstte oluşturduk .on ile bunun üstünde işlem yaptığımızı belirtiyoruz. click ile tıklama işlemi yapılacak diyoruz.
            //btnSave ile o id ye sahip butona tıklandığında alttaki fonksiyon işlemi yapılacak diyoruz.kategori ekleme sayfasındaki kaydet butonu id si.
            placeHolderDiv.on('click',
                '#btnSave',
                function (event) { //bu kod ile butona tıkladığımızda default yapılacak işlemleri durdurduk.Bizim belirttiğimiz işlemleri yapacak.
                    event.preventDefault();
                    //kategori ekle sayfasındaki bu id ye sahip formu aldık.
                    const form = $('#form-category-add');
                    //aldığımız formda asp-action olarak belirtilen yer neresiyse onun url ini alıyor.
                    const actionUrl = form.attr('action');
                    //bu formdaki değerleri direk o sayfada verdiğimiz modele dönüştürüyor.
                    const dataToSend = form.serialize();
                    //hangi actiona hangi datayı post edeceğimizi veriyoruz. Post işlemi yaptığımız aciton bize değer döndürüyor.
                    $.post(actionUrl, dataToSend).done(function (data) {
                        //json olarak gelen datayı tekrar model haline döndürüyoruz.
                        const categoryAddAjaxModel = jQuery.parseJSON(data);
                        //categoryaddpartial view içerisindeki sadece class =model-body olan kısmı al diyoruz çünkü bize sadece orası lazım.
                        const newFormBody = $('.modal-body', categoryAddAjaxModel.CategoryAddPartial);
                        //form içinde clası modal body olan kısmı yeni body ile değiştiriyoruz.
                        placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                        //form içinde modal-body'nin en üstünde name=IsValid olan bir satır var verinin valid değerlerinin doğru olup olmadığını kontrol ediyor.
                        //burda name ==IsValid olan yeri bul olnun value'su true ise true döndür değil ise false döndür diyor.
                        const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                        if (isValid) {
                            //girilen değerler doğru iste kategori ekleme ekranını kapatacaığımız için hide yapıyoruz.
                            placeHolderDiv.find('.modal').modal('hide');
                            //tabloya bu yeni veriyi eklemek için önce string şeklinde tablo satırını oluşturuyoruz.
                            const newTableRow = `
                                <tr name="${categoryAddAjaxModel.CategoryDto.Category.Id}">
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Id}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Name}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Description}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.IsActive ? "Evet" :"Hayır"}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.IsDeleted ? "Evet" : "Hayır"}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.Note}</td>
                                                    <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.CreatedDate)}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.CreatedByName}</td>
                                                    <td>${convertToShortDate(categoryAddAjaxModel.CategoryDto.Category.ModifiedDate)}</td>
                                                    <td>${categoryAddAjaxModel.CategoryDto.Category.ModifiedByName}</td>
                                <td>
                                    <button class="btn btn-primary btn-sm btn-update" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}><span class="fas fa-edit"></span></button>
                                    <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryAddAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-minus-circle"></span></button>
                                </td>
                                                </tr>`;
                            //string olarak oluşturduğumuz satırı js objesine çeviriyoruz.
                            const newTableRowObject = $(newTableRow);
                            //satırı yavaş yavaş ortaya çıkaracağımız için önce saklıyoruz.
                            newTableRowObject.hide();
                            //bu id ye sahip tabloya ekliyoruz yeni satırı.
                            $('#categoriesTable').append(newTableRowObject);
                            //Saklanan verinin yavaş yavaş görünmesini sağlıyor.1000=1sn
                            newTableRowObject.fadeIn(3500);
                            toastr.success(`${categoryAddAjaxModel.CategoryDto.Message}`, 'Başarılı İşlem!');
                        } else {
                            //Eğer validation hatası varsa ekleme sayfasındaki summaryi belirten divin id si üzerinde özeti alıp toastr içinde ekrana bastırıyoruz.
                            //summary ul içinde li ler ile geliyor her bir li farklı hata. Hepsini tek tek alıp texte atıyoruz. ul>li  (ul içindeki li demek)
                            let summaryText = "";
                            $('#validation-summary > ul > li').each(function () {
                                let text = $(this).text();
                                summaryText += `*${text}\n`;
                            });
                            toastr.warning(summaryText);
                        }

                    });
                });
        });
        /* Ajax POST / Posting the FormData as CategoryAddDto ends here */

        /* Ajax POST / Deleting a category starts from here */

        $(document).on('click',
            '.btn-delete',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                const tableRow = $(`[name="${id}"]`);
                const categoryName = tableRow.find('td:eq(1)').text();
                Swal.fire({
                    title: 'Silmek istediğinize emin misiniz?',
                    text: `${categoryName} adlı kategori silinecektir.`,
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Evet, silmek istiyorum.',
                    cancelButtonText: 'Hayır, silmek istemiyorum.'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            type: 'POST',
                            dataType: 'json',
                            data: { categoryId: id },
                            url: '/Admin/Category/Delete/',
                            success: function (data) {
                                const categoryDto = jQuery.parseJSON(data);
                                if (categoryDto.ResultStatus === 0) {
                                    Swal.fire(
                                        'Silindi!',
                                        `${categoryDto.Category.Name} adlı kategori başarıyla silinmiştir.`,
                                        'success'
                                    );
                                    tableRow.fadeOut(3500);
                                } else {
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Başarısız İşlem!',
                                        text: `${result.Message}`,
                                    });
                                }
                            },
                            error: function (err) {
                                console.log(err);
                                toastr.error(`${err.responseText}`, "Hata!");
                            }
                        });
                    }
                });
            });
    });

    /* Ajax POST / Deleting a category finish  here */

    /* Ajax GET / Getting the _CategoryUpdatePartial as Modal Form starts from here */

    $(function () {
        const url = '/Admin/Category/Update';
        const placeHolderDiv = $('#modalPlaceHolder');
        $(document).on('click',
            '.btn-update',
            function (event) {
                event.preventDefault();
                const id = $(this).attr('data-id');
                $.get(url, { categoryId: id }).done(function (data) {
                    placeHolderDiv.html(data);
                    placeHolderDiv.find('.modal').modal('show');
                }).fail(function () {
                    toastr.error("Bir Hata Oluştu.");
                });
            });

        /* Ajax POST / Updating a Category starts from here */
        
        placeHolderDiv.on('click',
            '#btnUpdate',
            function (event) {
                event.preventDefault();

                const form = $('#form-category-update');
                const actionUrl = form.attr('action');
                const dataToSend = form.serialize();
                $.post(actionUrl, dataToSend).done(function (data) {
                    const categoryUpdateAjaxModel = jQuery.parseJSON(data);
                    console.log(categoryUpdateAjaxModel);
                    const newFormBody = $('.modal-body', categoryUpdateAjaxModel.CategoryUpdatePartial);
                    placeHolderDiv.find('.modal-body').replaceWith(newFormBody);
                    const isValid = newFormBody.find('[name="IsValid"]').val() === 'True';
                    if (isValid) {
                        placeHolderDiv.find('.modal').modal('hide');
                        const newTableRow = `
                                <tr name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}">
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Id}</td>
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Name}</td>
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category
                                .Description}</td>
                                                     <td>${categoryUpdateAjaxModel.CategoryDto.Category.IsActive ? "Evet" : "Hayır"}</td>
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.IsDeleted ? "Evet" : "Hayır"}</td>
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category.Note}</td>
                                                    <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto
                                            .Category.CreatedDate)}</td>
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category
                                .CreatedByName}</td>
                                                    <td>${convertToShortDate(categoryUpdateAjaxModel.CategoryDto
                                    .Category.ModifiedDate)}</td>
                                                    <td>${categoryUpdateAjaxModel.CategoryDto.Category
                                .ModifiedByName}</td>
                                                    <td>
                                                        <button class="btn btn-primary btn-sm btn-update" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"><span class="fas fa-edit"></span></button>
                                                        <button class="btn btn-danger btn-sm btn-delete" data-id="${categoryUpdateAjaxModel.CategoryDto.Category.Id
                            }"><span class="fas fa-minus-circle"></span></button>
                                                    </td>
                                                </tr>`;
                        const newTableRowObject = $(newTableRow);
                        const categoryTableRow = $(`[name="${categoryUpdateAjaxModel.CategoryDto.Category.Id}"]`);
                        newTableRowObject.hide();
                        categoryTableRow.replaceWith(newTableRowObject);
                        newTableRowObject.fadeIn(3500);
                        toastr.success(`${categoryUpdateAjaxModel.CategoryDto.Message}`, "Başarılı İşlem!");
                    } else {
                        let summaryText = "";
                        $('#validation-summary > ul > li').each(function () {
                            let text = $(this).text();
                            summaryText = `*${text}\n`;
                        });
                        toastr.warning(summaryText);
                    }
                }).fail(function (response) {
                    console.log(response);
                });
            });
    });
});