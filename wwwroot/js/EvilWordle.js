export function TriggerModal() {
    $('#EvilWordleModal').modal('show')
}

export function IsDevice() {
    console.log(/android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent));
    return /android|webos|iphone|ipad|ipod|blackberry|iemobile|opera mini|mobile/i.test(navigator.userAgent);
}

