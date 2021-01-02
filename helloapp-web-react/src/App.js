import React from 'react';
import logo from './logo.svg';
import './App.css';

class App extends React.Component {
  constructor(props){
    super(props);

    this.showMessage = this.showMessage.bind(this);
    this.state = {
      apiCalled: false,
      isLoggedIn: false
    };
  }

  showMessage(){
    console.log("Show message called!");
  }


  render() {
    const isLoggedIn = this.state.isLoggedIn;
    let result;
    if(isLoggedIn){
      result = <div className="text-success" id="message-success">Hello from Web API</div>
    } else {
      result = <div className="text-danger"  id="message-error">Something went wrong</div>
    }

    return (
      <div className="text-center">
        <h1 className="display-4">Welcome</h1>
        
        <div className="row" id="divAction">
            <div className="col-12">
                <button type="button" className="btn btn-primary" onClick={this.showMessage} >Show Message</button>
            </div>
        </div>
        <hr />
        <div className="row" style={{marginTop:10 + 'px'}}>
          {this.state.apiCalled ? ( <div className="col-12"> {result} </div>) : (<div></div>)}
        </div>
      </div>
    );
  }
}

export default App;
