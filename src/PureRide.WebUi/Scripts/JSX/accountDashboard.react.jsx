 

var PersonalDetails = React.createClass({
    render: function() {
        return (
            <div>
            <h2 className="heading">{this.props.personalDetails.Name}</h2>
                <div>Email:{this.props.personalDetails.Email} </div>
                <div>Phone: {this.props.personalDetails.Phone} </div>
                <div>Shoe Size: {this.props.personalDetails.ShoeSize} </div>
                <div>Birthday: {this.props.personalDetails.Birthday} </div>
                <a href="/account/update-details/">Update Details</a>
                <a href="/account/update-password/">Update Password</a>
            </div>)
    }
});

var CreditStatus = React.createClass({
    getInitialState: function() {
        return {credits:null,loadedCredits:false};
    },
    componentDidMount: function() {
       this._loadFromServer();
    },
    _loadFromServer: function() {
        var context = this;
        reqwest({
            url: '/api/account/GetCreditBalance/',
            method: 'get',
            success: function(resp) {
                context.setState({ credits: resp.Data,loadedCredits:true });
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });
    },
    render: function() {

        var creditNodes = null;
        
        if(this.state.credits != null)
        {
            creditNodes = this.state.credits.map(function (creditSet) {
                return (<li key={creditSet.Region}>{creditSet.Region} {creditSet.Credits}</li>);
            });  
        }

        var message;
        if (creditNodes === null && this.state.loadedCredits) {
            message = "You have no credits.";
        }
    
    return (<div>
            <h2 className="heading">Credits</h2>
            <p>{message}</p>
            <ul>
            {creditNodes}
            </ul>
            <a href="/account/credits/">view details</a>
           </div>);

}});

var ScheduledClass = React.createClass({
    render: function() {

        return (<div> {this.props.scheduledClass[0].ScheduledClass.Name} </div>)
}});

var BookedClasses = React.createClass({
    getInitialState: function() {
        return {scheduledClasses: null, viewFullHistory: false,loadedBookings:false};
    },
    componentDidMount: function() {
        this._loadFromServer(false);
    },
    _loadFromServer: function(includePast) {
        var context = this;
        reqwest({
            url: '/api/account/GetBookings/?includePast=' + includePast.toString(),
            method: 'get',
            success: function(resp) {
                context.setState({ scheduledClasses: resp.Data.Bookings,viewFullHistory:includePast, loadedBookings:true });
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });
    },
    render: function() {
        var scheduledClassNodes = null;
        
        if(this.state.scheduledClasses !== null && this.state.scheduledClasses.length > 0)
        {
            scheduledClassNodes = this.state.scheduledClasses.map(function (scheduledClass) {
                return (<ScheduledClass key={scheduledClass.Key} scheduledClass={scheduledClass.Value} />);
            });
        }

        var loadAllButton = "";
        if(!this.state.viewFullHistory){
            loadAllButton = (<button className="btn btn-secondary" onClick={this._loadFromServer.bind(this, true)}>View Full History</button>);
        }

        var message;
        if (scheduledClassNodes === null && this.state.loadedBookings) {
            message = "You have no classes currenlty booked.";
        }

        return (
    <div>
    <h2 className="heading">Booked Classes</h2>
        <p>{message}</p>
        <ul>
        {scheduledClassNodes}
        </ul>
        {loadAllButton}
    </div>)
    }
});

var AccountDashboardContainer = React.createClass({
    render: function() {

        var self = this;

        return (
            <div>
                <div className="row">
                    <div className="col-md-6">
                        <PersonalDetails personalDetails={this.props.personalDetails} />
                    </div>  
                    <div className="col-md-6">
                        <CreditStatus />
                    </div>
                </div>
               <div className="row">  
                    <BookedClasses />
                </div>
        </div>
    );
}
});