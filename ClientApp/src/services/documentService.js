export default class FileService {
  getBase64FromFile = file => {
    return new Promise((resolve, reject) => {
      if (window.FileReader && window.Blob) {
        const reader = new FileReader();

        reader.onload = event => {
          resolve(event.target.result);
        };

        reader.onerror = reject;

        reader.readAsDataURL(file);
      } else {
        // eslint-disable-next-line prefer-promise-reject-errors
        reject('Ups... File upload is not supported for your device.');
      }
    });
  };

  base64ToBlob = (
    base64,
    contentType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
    sliceSize = 512
  ) => {
    const byteCharacters = atob(base64);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);

      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i += 1) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);

      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, {
      type: contentType
    });
    return blob;
  };

  processDownload = (fileName, base64, contentType) => {
    const blob = this.base64ToBlob(base64, contentType);
    if (window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveBlob(blob, fileName);
    } else {
      const elem = window.document.createElement('a');
      elem.href = window.URL.createObjectURL(blob);
      elem.style = 'display: none';
      elem.download = fileName;
      document.body.appendChild(elem);
      elem.click();
      document.body.removeChild(elem);
    }
  };
}
