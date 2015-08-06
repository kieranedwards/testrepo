/*
To show call: this.refs.dialog.showDialog();
To use: 

<ModalDialog ref="dialog"><h1>Content Title</h1><p>Content Message</p></ModalDialog>

<ModalDialog ref="dialog2"  actionButtonShown="true" actionButtonLabel="Add Friend" onAction={this._onDialogAction}><h1>Content Title</h1><p>Content Message</p></ModalDialog>

*/
var ModalDialog = React.createClass({
    getDefaultProps:function() {
        return {
            actionButtonShown:false,
            actionButtonLabel:""
        }
    },
    getInitialState:function () {
         return {top: 0, left: 0, dialogShown: false};
    },
    componentDidMount:function() {
        var self = this;
        document.on('keydown', function(e) {
            if (e.keyCode === 27) {
                self._dialogCloseRequest();
            }
        });
    },
    showDialog:function() {
        document.body.style.overflow = "hidden";
        this.setState({ dialogShown: true }); 
    },
    closeDialog:function() {
        document.body.style.overflow = "auto";
        this.setState({ dialogShown: false });
    },
    _dialogCloseRequest:function() {
        
        if(this.props.onClose !== undefined && this.props.onClose() === false){
            return;
        }

        this.closeDialog();
    },
    _onDialogClick:function(e) {
        if (e.target.className !== "dialog__container" && e.target.className !== "dialog") {
            return;
        }

        this._dialogCloseRequest();
    },
    _onDialogAction: function() {
        if (this.props.onAction !== undefined && this.props.onAction() === false) {
            return;
        }
     },
    render: function() {

        var actionButton = "";
        if(this.props.actionButtonShown){
            actionButton = <button className="btn btn--primary" onClick={this._onDialogAction} >{this.props.actionButtonLabel}</button>;
        }
    
        return (
            <div>
                <div className= { this.state.dialogShown ? "dialog": "dialog--hidden" } onClick={this._onDialogClick} >
                <div className="dialog__container">
                <div ref="dialogWindow" className="dialog__content">
                    {this.props.children}
                    {actionButton}
                    <button className="btn btn--secondary" onClick={this._dialogCloseRequest}>Close</button>
               </div>
               </div>
               </div>
            </div>
);
}
});