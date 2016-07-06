# FaceOff
FaceOff pits two players against eachother to see who can reflect an emotion best! 

The app will provide a random emotion that both players will portray. It then uses the facial recognition technology to determine who exhibited the emotion best!

## About
This is an Android and iOS app created using [Xamarin.Forms](https://www.xamarin.com/forms). The facial recognition uses [Microsoft's Coginitive Services Emotion API](https://www.microsoft.com/cognitive-services/). 

## ToDo
To access Microsoft's Coginitive Services Emotion API from this app, sign up for a [free API Key](https://www.microsoft.com/cognitive-services/en-us/emotion-api) and insert it to the code [here](https://github.com/brminnick/FaceOff/blob/master/Constants/CognitiveServicesConstants.cs#L8). After adding your API Key, remove the diagnostic directive [located here](https://github.com/brminnick/FaceOff/blob/master/Constants/CognitiveServicesConstants.cs#L6).

### Author
Brandon Minnick

Xamarin Customer Success Engineer


![FaceOff Demo](./Demos/FaceOff_GifDemo.gif)


### Licensing
MIT License

Copyright (c) 2016 Brandon Minnick

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Data submitted may be retained by Microsoft for service improvement purposes, along with the other stipulations noted in the following documents: [Cognitive Services TOU](https://go.microsoft.com/fwlink/?LinkId=533207), [Developer Code of Conduct](http://go.microsoft.com/fwlink/?LinkId=698895)
