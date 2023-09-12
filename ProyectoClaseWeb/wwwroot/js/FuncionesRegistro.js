//funcion ajax para verificar si el correo esta o no en la bd
//la funcion se paso a usar con jquery, getelementbyid = $ y cuando se usa el dolar pues hay que ponerle los # a los id, jquery es un poco mas abstracto
//ademas ya no se pone .value sino que en jquery se usa .val()
function BuscarCorreo() {

    let CorreoElectronico = $("#CorreoElectronico").val();
    $("#btnRegistrar").prop("disabled", true);//aqui lo sigo deshabilitando

    if (CorreoElectronico.trim() != "") //cuando el correo no sea vacio ni al principio o al final
    {
        $.ajax({
            url: '/Home/BuscarCorreo',//ruta del controlador(en este caso el es nuestro servidor)
            data: { "CorreoElectronico": CorreoElectronico },
            type: 'GET',
            dataType: 'json',//85% respuestas de ajax son json..
            success: function (res) {//success es el que captura la respuesta del servidor.
                if (res == "")
                    $("#btnRegistrar").prop("disabled", false);//aqui lo habilito
                else
                    alert(res);
            }
        });
    }
}
