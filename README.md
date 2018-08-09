# VAE Bigram Parser

This solution is used for parsing nGrams (bigrams more specifically) via text or file upload. The main logic will create a dictionary of nGrams (and their total number of occurences) and output the results to a histogram.

## Background

While the purpose of this project was to simply parse bigrams, it has been designed in a more scalable fasion such that 'n' can be changed to any number greater than 0. This functionality was not transfered to the UI as it was not part of the initial business requirements, but it would be very simple to allow users to choose the nGram size.

## Input

The logic allows for any text ot file type, but the output may become confusing if an image were to be uploaded opposed to a text file. As mentioned above, the logic is set to parse bigrams so providing anything less than 2 words would result in an error diplayed to the user. For convenience, there is an 'onchange' event attached to the text and file input fields so the form will submit right as a change is made rather than having to click a submit button.
