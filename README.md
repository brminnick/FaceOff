[![Build Status](https://www.bitrise.io/app/3dca187dd687b3ab/status.svg?token=blpNUgDzrfaB73rNn5I0TA&branch=master)](https://www.bitrise.io/app/3dca187dd687b3ab)

# FaceOff
FaceOff pits two players against eachother to see who can reflect an emotion best! 

The app will provide a random emotion that both players will portray. It then uses the facial recognition technology to determine who exhibited the emotion best!

## About
This is an Android, iOS and UWP app created using [Xamarin.Forms](https://www.xamarin.com/forms). The facial recognition uses [Microsoft's Coginitive Services Emotion API](https://aka.ms/Myvcp0). 

## ToDo
To access Microsoft's Coginitive Services Emotion API from this app, sign up for a [free API Key](https://aka.ms/myvcp0) and insert it to the code [here](./FaceOff/Constants/CognitiveServicesConstants.cs#L7). After adding your API Key, remove the diagnostic directive [located here](./FaceOff/Constants/CognitiveServicesConstants.cs#L5).

### Author
Brandon Minnick

Xamarin Customer Success Engineer


![FaceOff Demo](https://github.com/brminnick/Videos/blob/master/FaceOff/FaceOff_GifDemo.gif)
