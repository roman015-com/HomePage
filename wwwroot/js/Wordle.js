
export function TriggerModal() {
    $('#WordleModal').modal('show')
}

export function HideModal() {
    $('#WordleModal').modal('hide')
}

export function IsDevice() {
    //console.log(/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent));
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}

export function CopyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        
    })
    .catch(function (error) {
        alert("Unable to copy link to clipboard");
    });
}

