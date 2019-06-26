import React, { Component } from 'react';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import Loader from './sections/Loader';

// eslint-disable-next-line react/prefer-stateless-function
export default class App extends Component {
  constructor(props, context) {
    super(props, context);

    this.state = {
      isLoading: false
    };
  }

  showLoader = () => {
    this.setState({ isLoading: true });
  };

  hideLoader = () => {
    this.setState({ isLoading: false });
  };

  render() {
    const { isLoading } = this.state;
    return (
      <Layout>
        <Route
          exact
          path="/"
          render={routeProps => (
            <Home {...routeProps} showLoader={this.showLoader} hideLoader={this.hideLoader} />
          )}
        />
        ,
        <ToastContainer />,
        <Loader isLoading={isLoading} />
      </Layout>
    );
  }
}
