//<script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/rollups/hmac-sha256.js"></script>
//<script src="http://crypto-js.googlecode.com/svn/tags/3.0.2/build/components/enc-base64-min.js"></script>
var SecurityManager = {

    generate: function (username, key) {

        // Get the (C# compatible) ticks to use as a timestamp. http://stackoverflow.com/a/7968483/2596404
        var ticks = ((new Date().getTime() * 10000) + 621355968000000000);

        // Construct the hash body by concatenating the username, ip, and userAgent.
        var message = [username, ticks].join(':');

        // Hash the body, using the key.
        var hash = CryptoJS.HmacSHA256(message, key);

        // Base64-encode the hash to get the resulting token.
        var token = CryptoJS.enc.Base64.stringify(hash);

        // Include the username and timestamp on the end of the token, so the server can validate.
        var tokenId = [username, ticks].join(':');

        // Base64-encode the final resulting token.
        var tokenStr = CryptoJS.enc.Utf8.parse([token, tokenId].join(':'));

        return CryptoJS.enc.Base64.stringify(tokenStr);
    }
};