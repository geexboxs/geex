import * as nodemailer  from "nodemailer";
import * as Mail  from "nodemailer/lib/mailer";
import * as SMTPConnection  from "nodemailer/lib/smtp-connection";
export class EmailSender {
    displayAs: Mail.Address;
    transport: Mail;

    /**
     *
     */
    constructor(displayAs: Mail.Address, options: SMTPConnection.Options) {
        this.displayAs = displayAs;
        this.transport = nodemailer.createTransport(options);
        this.transport.verify((error, success) => {
            if (error) {
                throw error;
            } else {
                console.log("EmailSender ready");
            }
        });
    }

    async send(to: [Mail.Address], content: string, contentType: "text" | "html", attachments?: [Mail.Attachment], cc?: [Mail.Address], bcc?: [Mail.Address]) {
        return await new Promise<void>((resolve, reject) => {
            const options = {
                to,
                text: contentType === "text" ? content : undefined,
                html: contentType === "html" ? content : undefined,
                from: this.displayAs,
            };
            if (attachments) {
                options["attachments"] = attachments;
            }
            if (cc) {
                options["cc"] = cc;
            }
            if (bcc) {
                options["bcc"] = bcc;
            }
            this.transport.sendMail(options, (err, info) => {
                err === null ? resolve() : reject(err);
            });
        });

    }
}
