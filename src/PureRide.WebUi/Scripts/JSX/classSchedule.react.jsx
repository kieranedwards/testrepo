var ClassCard = React.createClass({
    render: function() {
        return (<div className={this.props.scheduledClass.IsBookingUnavailable ? "schedule-card schedule-card--unavailable" : "schedule-card"} onClick={this.props.onClassSelected.bind(this, this.props.scheduledClass)}>     
    <div>{this.props.scheduledClass.InstructorName}</div>
    <div>{this.props.scheduledClass.ClassName}</div>
    <div>{this.props.scheduledClass.RoomName}</div>
    <div className="schedule-card__time">{this.props.scheduledClass.ClassTime}</div>
        <div className="schedule-card__overlay">xxxx</div>
     </div>);
    }
});


var DayColumn = React.createClass({
    render: function() {
        var classSelectedMethod = this.props.onClassSelected;
        var classNodes = this.props.data.map(function (scheduledClass) {
            return (<ClassCard key={scheduledClass.ClassId} id={scheduledClass.ClassId} scheduledClass={scheduledClass} onClassSelected={classSelectedMethod} ></ClassCard>);
});
return (
  <div className={this.props.isHidden ? "schedule-column--hidden" : "schedule-column"}>
    <div className="schedule-column__header"> {this.props.title} </div>
    {classNodes}
  </div>
);
}
});
 
var DayColumns = React.createClass({
    render: function() {
        var classSelectedMethod = this.props.onClassSelected;
        var dayColumnNodes = this.props.data.map(function(day) {
            return (<DayColumn key={day.Date} title={day.DisplayName} isHidden={day.isHidden}  data={day.Classes} onClassSelected={classSelectedMethod} ></DayColumn>)
        });
    
return (
<div className="row" >
    {dayColumnNodes}
</div>
);
}
});

var Navigation = React.createClass({
    render: function() {
return (
<div className="row" >
    <div className="col-md-6">
        <a href="#" onClick={this.props.movePrevious}>Previous Dates</a>
     </div>
     <div className="col-md-6">
        <a href="#" onClick={this.props.moveNext}>Future Dates</a>
     </div>
</div>
);
}
});

var ClassScheduleContainer = React.createClass({
    getInitialState: function() {
        return this._setDaysVisible(this._createState(0,7));
    },
    _onClassSelected:function (selectedClass) {

        var msg = this.props.messages[selectedClass.AvailabilityStatus];
        if (msg == null || msg.Title.length===0){
            window.location = selectedClass.ClassName.toLowerCase().replace(" ", "-") + "-" + selectedClass.ClassId.split(":")[1];
        } else {
            this.setState({message:msg});
            this.refs.dialog.showDialog();
        }
    },
    _createState:function (start,end,message) {
        return {viewStart: start, viewEnd: end, classes:this.props.classes, message: message|| {Title:"Loading...", Message:"Loading..."} };
    },
    _moveNext: function() {
        this.setState(this._setDaysVisible(this._createState(this.state.viewStart+7,this.state.viewEnd+7)));
    },
    _movePrevious: function() {
        this.setState(this._setDaysVisible(this._createState(this.state.viewStart-7,this.state.viewEnd-7)));
    },
    _setDaysVisible(state) {
        for (var i = 0; i <  state.classes.length; i++) {
            state.classes[i].isHidden = (i < state.viewStart || i > state.viewEnd);
        }
        return state;
    },
    render: function() {
        return (
          <div>
            <Navigation movePrevious={this._movePrevious} moveNext={this._moveNext}/>
            <DayColumns data={this.state.classes} onClassSelected={this._onClassSelected} />
            <Navigation movePrevious={this._movePrevious} moveNext={this._moveNext}/>
            <ModalDialog ref="dialog">
                  <h2>{this.state.message.Title}</h2>
                  <p>{this.state.message.Message}</p>
            </ModalDialog>
        </div>
    );
}
});

