import React, { Component } from 'react';
import Dropzone from 'react-dropzone';
import { toast } from 'react-toastify';
import DocumentService from '../services/documentService';
import './Home.css';

export class Home extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {};
  }

  onPhotoFileDrop = async files => {
    if (files.length > 0) {
      const { showLoader, hideLoader } = this.props;
      showLoader();

      const documentService = new DocumentService();
      const fileName = files[0].name;
      const fileContentType = files[0].type;

      const base64Data = await documentService.getBase64FromFile(files[0]);

      fetch('api/Document/EditDocument', {
        method: 'POST',
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({ base64: base64Data, fileName })
      })
        .then(response => {
          return response.json();
        })
        .then(wordDoc => {
          const { base64 } = wordDoc;

          documentService.processDownload(fileName, base64, fileContentType);

          hideLoader();
        })
        .catch(error => {
          hideLoader();

          toast.error(`Could not edit the document - ${error}`, {
            autoClose: true,
            position: toast.POSITION.TOP_CENTER
          });
        });
    }
  };

  render() {
    return (
      <div className="text-center">
        <h1>Word Document Editor</h1>
        <p>Welcome to Word Document Editor, online word document editor</p>

        <Dropzone
          onDrop={this.onPhotoFileDrop}
          accept=".doc, .docx"
          multiple={false}
          // max upload size is 8mb
          maxSize={8388608}
        >
          {({ getRootProps, getInputProps }) => (
            <section className="dropzone-container">
              <div {...getRootProps({ className: 'dropzone mt-15 pointer' })}>
                <input {...getInputProps()} />
                <p>Drag word document, or click to select</p>
              </div>
            </section>
          )}
        </Dropzone>
      </div>
    );
  }
}
