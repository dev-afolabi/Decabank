import 'bootstrap/dist/css/bootstrap.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

//  Create browser history to use in the Redux store
//  const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
//  const history = createBrowserHistory({ basename: baseUrl });


ReactDOM.render(<App />, document.getElementById('root'));

registerServiceWorker();
