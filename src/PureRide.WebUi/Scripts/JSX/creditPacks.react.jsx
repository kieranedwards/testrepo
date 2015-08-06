var Pack = React.createClass({
    handleClick: function(event) {
 
        reqwest({
            url: '/api/credits/select?productId='+this.props.id,
            method: 'post',
            success: function(resp) {
                window.location.href = resp.Data.RedirectUrl;      
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });
        
    },
    render: function() {
        return (
          <div className= {this.props.pack.IsSpecialOffer?"tmp_creditpack tmp_creditpackoffer":"tmp_creditpack"} onClick={this.handleClick}>
            
            <h2>
              {this.props.pack.Title} 
            </h2>
             <div>{this.props.pack.Notes}</div>
             <div>{this.props.pack.Quantity} Credits</div>
          </div>
      );
    }
});

var CampaignCodeForm = React.createClass({
    _onSubmit:function(e) {
        e.preventDefault();
        this.props.applyCreditPacksCode(this.refs.CampaignCode.getDOMNode().value);
    },
    render: function() {
        return (<form onSubmit={this._onSubmit}><label>Voucher Code</label><input type="text" maxLength="25" ref="CampaignCode"/><button type="submit" className="btn btn--primary">Apply</button></form>);
}
});

var CreditPackList = React.createClass({
    render: function() {
        var creditPackNodes = this.props.data.map(function (pack) {
            return (<Pack key={pack.ProductId} id={pack.ProductId} pack={pack}></Pack>);
});
return (
  <div>
    {creditPackNodes}
  </div>
);
}
});
 
var CampaignCodeMessage = React.createClass({
    getDefaultProps:function() {
        return {
            isValid:false,
            code:""
        }
    },
    render: function() {

        if (this.props.code == null || this.props.code.length === 0)
            return false;

        if (this.props.isValid) {
            return (<p>The code {this.props.code} has been applied.</p>)
        } else {
            return (<p>The code {this.props.code} is not valid and could not be applied.</p>)
        }

    }
});


var CreditPackContainer = React.createClass({
    getInitialState: function() {
        return {data: {AvailableCreditPacks:this.props.packs}};
    },
    _loadCreditPacksFromServer: function(campaignCode) {
        var context = this;
        reqwest({
            url: this.props.url,
            method: 'get',
            data: { code:campaignCode},
            success: function(resp) {
                context.setState({ data: resp.Data });
            },
            error: function(err,msg) {
                document.ajaxError(this,err,msg);
            }
        });
    },
    render: function() {
        return (
          <div>
            <CampaignCodeMessage isValid={this.state.data.IsPromotionalCodeValid} code={this.state.data.PromotionalCode} />
            <CreditPackList data={this.state.data.AvailableCreditPacks} />
            <CampaignCodeForm applyCreditPacksCode={this._loadCreditPacksFromServer} />
         </div>
    );
    }
});

