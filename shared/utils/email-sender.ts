import SmtpClient from "emailjs-smtp-client";
export class EmailSender extends SmtpClient {

    /**
     *
     */
    constructor(host: string, port: number, options: SmtpConnectOptions) {
        super();

    }
}
/**
 *
 * ## Connection options
 *
 * The following connection options can be used with `simplesmtp.connect`:
 *
 * * **useSecureTransport** *Boolean* Set to true, to use encrypted connection
 *
 * * **name** *String* Client hostname for introducing itself to the server
 *
 * * **auth** *Object* Authentication options. Depends on the preferred authentication method
 *
 *   * **user** is the username for the user (also applies to OAuth2)
 *
 *   * **pass** is the password for the user if plain auth is used
 *
 *   * **xoauth2** is the OAuth2 access token to be used instead of password. If both password and xoauth2 token are set, the token is preferred.
 *
 *   * **authMethod** *String* Force specific authentication method (eg. `"PLAIN"` for using `AUTH PLAIN` or `"XOAUTH2"` for `AUTH XOAUTH2`)
 *
 * * **ca** (optional) (only in conjunction with this [TCPSocket shim](https://github.com/emailjs/emailjs-tcp-socket)) if you use TLS with forge, 
 * pin a PEM-encoded certificate as a string. Please refer to the [tcp-socket documentation](https://github.com/emailjs/emailjs-tcp-socket) for more information!
 *
 * * **disableEscaping** *Boolean* If set to true, do not escape dots on the beginning of the lines
 *
 * * **ignoreTLS** – if set to true, do not issue STARTTLS even if the server supports it
 *
 * * **requireTLS** – if set to true, always use STARTTLS before authentication even if the host does not advertise it. If STARTTLS fails, do not try to authenticate the user
 *
 * * **lmtp** - if set to true use LMTP commands instead of SMTP commands
 *
 * Default STARTTLS support is opportunistic – if the server advertises STARTTLS in EHLO response, the client tries to use it. If STARTTLS is not advertised, 
 * the clients sends passwords in the plain. You can use `ignoreTLS` and `requireTLS` to change this behavior by explicitly enabling or disabling STARTTLS usage.
 *
 * ### XOAUTH2
 *
 * To authenticate using XOAUTH2, use the following authentication config
 *
 * ```javascript
 * var config = {
 *   auth: {
 *     user: 'username',
 *     xoauth2: 'access_token'
 *   }
 * }
 * ```
 *
 * See [XOAUTH2 docs](https://developers.google.com/gmail/xoauth2_protocol#smtp_protocol_exchange) for more info.
 *
 * @export
 * @interface SmtpConnectOptions
 */
export interface SmtpConnectOptions {
    [key: string]: any;
}
