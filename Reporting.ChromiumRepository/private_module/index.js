
const htmlPdf = require('html-pdf-chrome');
var fs = require('fs');

const htmlPath = process.argv[2];
const pdfPath = process.argv[3];
const html =  fs.readFileSync(htmlPath, 'utf8');;
const options = {
    //port: 9222,
    printOptions: {
        displayHeaderFooter: false,
        headerTemplate: '',
        footerTemplate: '',
        completionTrigger: new htmlPdf.CompletionTrigger.Event(
            'reporting-engine-has-finished', // name of the event to listen for
            '#my_reporting_finished', // optional DOM element CSS selector to listen on, defaults to body,
                10000
        ),
    },
};

htmlPdf.create(html, options).then((pdf) => {
    pdf.toFile(pdfPath);
});
