var Location = React.createClass({
    render: function() {
        var detailsUrl = "\\" + this.props.location.Name.toLowerCase().replace(" ", "-") + "-studio\\";
        var classesUrl = detailsUrl + "classes\\";
        return (
          <li>
            <div>{this.props.location.Name}</div>
            <div>{this.props.location.AddressLine1}</div>
            <div>{this.props.location.AddressLine2}</div>
            <div>{this.props.location.AddressLine3}</div>
            <div>{this.props.location.Postcode}</div>
            <div>{this.props.location.Email}</div>
            <div>{this.props.location.PhoneNumber}</div>

            <div><a href={classesUrl}>View Classes</a></div>
            <div><a href={detailsUrl}>More Details</a></div>

          </li>
      );
    }
});

var LocationList = React.createClass({
    render: function() {
        var locationNodes = this.props.locations.map(function (location) {
            return (
              <Location key={location.Name} location={location}>
          </Location>
      );
});
return (
  <ul>
    {locationNodes}
  </ul>
);
}
});

var RegionTabs  = React.createClass({
    render: function() {
        var self = this;
        var regionNodes = this.props.regions.map(function (region) {
            return (
              <li key={region}><a href="#" onClick={self.props.onTabClick}>{region}</a></li>
      );
});

if (this.props.regions.length === 1)
    return false;

return (
  <ul>
    {regionNodes}
  </ul>
);
}
});

var LocationMap  = React.createClass({
    componentDidMount:function() {
        var mapOptions = {zoom: 12,center: this._getLocations()[0].latLng};
        this.map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
        this.infowindow = new google.maps.InfoWindow();
        this.markers = [];
        
        var self = this;
        google.maps.event.addListenerOnce(this.map, 'idle', function() {
            self._dropMarkers();
        });
    },
    componentDidUpdate:function(prevProps,prevState) {
        this.map.panTo(this._getLocations()[0].latLng);
        this._dropMarkers();
    },
    _getLocations : function() {
        return this.props.locations.map(function(location) {
            return { latLng: new google.maps.LatLng(location.Latitude, location.Longitude),  title: location.Name};
        });
    },
    _dropMarkers:function() {
        this._clearMarkers();

        var mapPoints = this._getLocations();
        var initalPause = 500;

        for (var i = 0; i < mapPoints.length; i++) {
            this._addMarkerWithTimeout(mapPoints[i], (i * 200) + initalPause);
        }
    },
    _addMarkerWithTimeout : function (data, timeout) {
        var self = this;

        var marker = new google.maps.Marker({
            position: data.latLng,
            title: data.title,
            map: self.map,
            animation: google.maps.Animation.DROP,
            id: data.title
        });

        window.setTimeout(function() {
            self.markers.push(marker);
            self._attachMarkerMessage(marker);

        }, timeout);
    },
    _attachMarkerMessage: function(marker) {
        var self = this;

        google.maps.event.addListener(marker, 'click', function() {
            self.infowindow.setContent(marker.id);
            self.infowindow.open(marker.get('map'), marker);
        });
    },
    _clearMarkers :function() {
        for (var i = 0; i < this.markers.length; i++) {
            this. markers[i].setMap(null);
        }
        this.markers = [];
    },
    render: function() {
        return false;
    }
});

var LocationContainer = React.createClass({
    getInitialState: function() {
        return { selectedRegion: this.props.regions[0] };
    },
    componentDidMount: function() {
        var regionHash = window.location.hash.substr(1);

        if (this._isHashValidRegion(regionHash) && this.state.selectedRegion !== regionHash) {
            this.setState({ selectedRegion: regionHash });
        }
    },
    _isHashValidRegion:function(hash) {
        return hash.length > 0 && this.props.locations.filter(function(item) { return item.Region.toLowerCase() === hash.toLowerCase() });
    },
    _onRegionChangeRequest:function(e) {
        e.preventDefault();
        var regionName = e.target.textContent;
        location.hash = regionName;
        this.setState({selectedRegion:regionName});
    },
    render: function() {

        var self = this;
        var currentLocations = this.props.locations.filter(function(item) {return item.Region.toLowerCase() ===  self.state.selectedRegion.toLowerCase()});

        return (
          <div>
            <LocationMap locations={currentLocations} />
            <RegionTabs regions={this.props.regions} onTabClick={this._onRegionChangeRequest} />
            <LocationList locations={currentLocations} />
      </div>
    );
}
});