import React from 'react';
import { ClipLoader } from 'react-spinners';

// eslint-disable-next-line react/prefer-stateless-function
class Loader extends React.Component {
  render() {
    const { isLoading } = this.props;
    return (
      <div className={`progressbar-wrapper ${isLoading ? 'display-block' : 'display-none'}`}>
        <div className="progress-spinner">
          <ClipLoader sizeUnit="px" size={45} color="#007bff" loading={isLoading} />
        </div>
      </div>
    );
  }
}

export default Loader;
