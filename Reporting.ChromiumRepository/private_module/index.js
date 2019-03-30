
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
        completionTrigger: new htmlPdf.CompletionTrigger.Timer(10000),
    },
};

htmlPdf.create(html, options).then((pdf) => pdf.toFile(pdfPath));