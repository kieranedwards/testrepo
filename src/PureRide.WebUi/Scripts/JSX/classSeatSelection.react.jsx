 
var ActionButton = React.createClass({
    _onButtonClick:function() {

        if (this._isDisabled())
            return;

        if (this.props.loginRequired) {
            window.location.href = "/account/login/?returnurl=" + encodeURIComponent(window.location.pathname);
        } else {
            this.props.submitBooking();
        }

    },
    _getButtonText:function() {
        var buttonText = "Book Class";

        if (this.props.loginRequired) {
            buttonText = "Login or Register";
        }else if (this._isDisabled()) {
            buttonText = "Select Your Seat";
        }else if (this.props.creditsRequired) {
            buttonText = "Buy Credits and Book";
        }

        return buttonText;
    },
    _isDisabled:function() {
        return  !this.props.loginRequired && !this.props.userSeatSelected;
    },
    render: function() {

        if (this.props.locked)
            return false;
        
        var buttonText = this._getButtonText();
        var buttonClassName = (this._isDisabled() ? "btn btn--disabled" : "btn btn--primary");

        return (
            <button className={buttonClassName}  onClick={this._onButtonClick} >{buttonText}</button>
      )
    }
});

var AddFriendForm = React.createClass({
    render: function() {
        return (
            <form id="addfriend">
            <label for="firstname">First Name</label>
            <input name="firstname" id="firstname" required autocomplete="given-name" type="text" />
            <label for="lastname" id="lastname">Last Name</label>
            <input name="lastname" required autocomplete="family-name" type="text" />
            <label for="email">Email</label>
            <input name="email" type="email" />
            <label for="mobilephonenumber">Mobile Phone Number</label>
            <input name="mobilephonenumber" pattern="(07|\+447)\d{9}" autocomplete="tel" type="tel" />
            </form>
        );
    }
});

var SelectFriendList = React.createClass({
    render: function() {
        var onClickName = this.props.onAddFriend;
        var friendItemNodes = this.props.recentFriends.map(function (friend) {
            return (<li><span onClick={onClickName.bind(this,friend.FriendId)}>{friend.Name}</span></li>)
        });
        return (<ul>{friendItemNodes}</ul>);
    }
});

var AddFriendDialog = React.createClass({
    getInitialState: function() {
        return {showAdd: false, message:""};
    },
    showDialog:function() {
        this.refs.addDialog.showDialog();
    },
    _onDialogClose: function() {
        this.setState({showAdd: false, message:""});
    },
    _getFormData:function() {
        var formData = {};
        $('#addfriend input').forEach(function(el) {
            formData[el.name] = el.value;
        });
        return formData;
    },
    _clearFormData:function() {
        $('#addfriend input').forEach(function(el) {
            el.value = "";
        }); 
    },
    _onAddNewFriend:function() {

        if (!$("#addfriend")[0].checkValidity()) {
            this.setState({message:"Both the first name and last name are required. If a phone or email is provided it must be valid."});
            return false;
        }

        var self = this;
        reqwest({
            url: '/api/account/addfriend/',
            method: 'post',
            data: self._getFormData(),
            success: function(resp) {
                if (resp.Data === null) {
                    self.setState({message:"You cannot use your email address for a friends account."});
                } else {
                    self._onAddFriend(resp.Data);
                }
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });

    },
    _onAddFriend:function(id) {

        var self = this;
        reqwest({
            url: '/api/booking/isbooked/',
            method: 'post',
            data: {classId: this.props.classId,personId:id},
            success: function(resp) {

                if (resp.Data === true) {
                    self.setState({message: "The friend you have selected is already scheduled to attend a class at this time." });
                } else {
                    self.props.onFriendAdded(id);
                    self._clearFormData();
                    self.setState({ showAdd: false, message:"" });
                    self.refs.addDialog.closeDialog();
                }
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });

    },
    _showAddNew:function() {
        this.setState({showAdd: true, message:"" });
    },
    render: function() {

        var message;

        if (this.state.message.length > 0) {
            message = (<p className="field-validation-error">{this.state.message}add</p>);
        }

        var addDialog = 
                (
                <ModalDialog ref="addDialog" actionButtonShown="true" actionButtonLabel="Add Friend" onAction={this._onAddNewFriend} onClose={this._onDialogClose}>
                    <h2>Add a Friend</h2>
                    {message}
                    <p>Please enter the details of your new friend below. Entering your friends email will allow them to be be notified of the booking and be able to self signin when they arrive.Please note all friends must be over 18 to enjoy a pureride class.</p>
                    <AddFriendForm />
                </ModalDialog>
                );

        var listDialog = (
           <ModalDialog ref="addDialog" >
                <h2>Select a Friend</h2>
                {message}
                <SelectFriendList recentFriends={this.props.recentFriends} onAddFriend={this._onAddFriend} />
                <button onClick={this._showAddNew} className="btn btn--primary">Add New Friend</button>
           </ModalDialog>
          );

        if (this.props.recentFriends.length===0 || this.state.showAdd) {
            return addDialog;
        } else {
            return listDialog;
        }
    }
});

var CreditCount = React.createClass({
    render: function() {
        return (
          <div>
              Credits Needed: {this.props.creditsRequired}  
              Credits Available: {this.props.creditBalance} 
          </div>
      );
}
});

var Seat = React.createClass({
    _handleClick: function(event) {
        if (this.props.seat.IsAvailable) {
            this.props.onSeatSelected(this.props.seat);
        }
    },
    render: function() {
       
        var cssStyle = {left: (this.props.seat.X * this.props.xMultiplier) + "px", top: (this.props.seat.Y * this.props.yMultiplier) + "px" };
        var classes = new Array("tmp_seat");
        var selectedSeatId = this.props.seat.SeatId;

        if (this.props.seat.IsInstructor) 
            classes.push("tmp_instuctor");
        else if(!this.props.seat.IsAvailable)
            classes.push("tmp_taken");
        else if(selectedSeatId === this.props.userSeatId)
            classes.push("tmp_seat_selected");
        else if(this.props.friends.some(function(item) { return item.seatId === selectedSeatId }))
            classes.push("tmp_seat_selected_friend");

        var className = classes.join(' ');

        return (
          <div className={className}  style={cssStyle} onClick={this._handleClick}>
              {this.props.seat.SeatId} 
          </div>
      );
    }
});

var SeatGrid = React.createClass({
    render: function() {
        var userSeatId = this.props.userSeatId;
        var friends = this.props.friends;
        var onSeatSelected = this.props.onSeatSelected;

        var maxX=0, maxY = 0;
        for (var i = 0; i < this.props.data.length; i++) {
            if (this.props.data[i].X > maxX)
                maxX = this.props.data[i].X;

            if (this.props.data[i].Y > maxY)
                maxY = this.props.data[i].Y;
        }

        var xMultiplier = 1000/maxX;
        var yMultiplier = 600/maxY ;
        
        var seatNodes = this.props.data.map(function (seat) {
            return (<Seat key={seat.SeatId} id={seat.SeatId} seat={seat} xMultiplier={xMultiplier} yMultiplier={yMultiplier} userSeatId={userSeatId} friends={friends} onSeatSelected={onSeatSelected}></Seat>)
        });
return (<div className="tmp_container">{seatNodes}</div>);
}
});


var SeatSelectionContainer = React.createClass({
    getInitialState: function() {
        return {userSeatId: 0, selectedFriends: [], pendingSeatId:0};
    },
    _onFriendAdded:function(id) {
        if (this.state.pendingSeatId !== 0) {
            this.setState({selectedFriends: this.state.selectedFriends.concat({seatId: this.state.pendingSeatId, friendId:id}), pendingSeatId:0 });
        }
    },
    _onSeatSelected:function (selectedSeat) {

        var usersSeatId = this.state.userSeatId;
        var selectedFriends = this.state.selectedFriends;
        var selectedSeatId = selectedSeat.SeatId;

        if (this.props.locked)
            return;

        if ("Instructor" === selectedSeatId) {
            return;
        }

        if (usersSeatId === selectedSeatId) {
            this.setState({ userSeatId: 0 });
            return;
        }

        if (usersSeatId === 0) {
            this.setState({ userSeatId: selectedSeat.SeatId });
            return;
        }

        var findSeatInFriends = selectedFriends.some(function(item) { return item.seatId === selectedSeatId });

        if (findSeatInFriends){
            this.setState({ selectedFriends: selectedFriends.filter(function(item) { return item.seatId !== selectedSeatId }) }); 
            return;
        }

        if (this.state.selectedFriends.length >= 3) {
            this.refs.warningDialog.showDialog();
        } else {
            this.setState({pendingSeatId:selectedSeatId}); 
            this.refs.addFriendDialog.showDialog();
        }
    },
    _onSubmitBooking:function() {

        var bookingData = {ClassId:this.props.classId, SeatId:this.state.userSeatId, Friends: this.state.selectedFriends};
        var self = this;
        reqwest({
            url: '/api/booking/placebooking/',
            method: 'post',
            data: bookingData,
            success: function(resp) {
                window.location.href = resp.Data.Url;
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });

    },
    render: function() {

        var creditsRequiredCount = this.state.selectedFriends.length + (this.state.userSeatId === 0 ? 0 : 1);
        var isUserSeatSelected = this.state.userSeatId !== 0;
        var isCreditsRequired = creditsRequiredCount > this.props.creditBalance;

        var selectedFriends = this.state.selectedFriends;
        var availableFriends = this.props.recentFriends.filter(function(friend) {
            return !selectedFriends.some(function(bookedFriend) {
                        return friend.FriendId === bookedFriend.friendId;
                    });
        });

        return (
          <div>
            <CreditCount creditBalance={this.props.creditBalance}  creditsRequired={creditsRequiredCount}/>
            <SeatGrid data={this.props.seats} userSeatId={this.state.userSeatId} friends={this.state.selectedFriends} onSeatSelected={this._onSeatSelected}/>
            <ActionButton userSeatSelected={isUserSeatSelected} creditsRequired={isCreditsRequired} loginRequired={this.props.loginRequired} locked={this.props.locked} submitBooking= {this._onSubmitBooking} />
            <AddFriendDialog ref="addFriendDialog" onFriendAdded={this._onFriendAdded} recentFriends={availableFriends} classId={this.props.classId} />
            <ModalDialog ref="warningDialog"><h1>No more friends</h1></ModalDialog>
        </div>
        );
}
});

