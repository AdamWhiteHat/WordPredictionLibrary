# WordPredictionLibrary<br/>
* Tracks the frequency of words that follow other words.<br/>
* You must train up the dictionary by supplying it with a body of text.<br/>
* Stores each word seen, and each word that follows each given word.<br/>
* Allows for serializing/deserializing of entire data structure.<br/>
* Stores the raw word counts, rather than a percentage or probability, because a running probability would introduce an ever-increasing error.<br/>
* Many diffrent possible predictions can be drawn from the data:<br/>
  * Simple next-word probability or suggestion<br/>
  * Analyzing a body of text for randomness by examining its next-word probabilies.<br/>
  * Word collocation analysis<br/>
  * Single/Unique word analysis<br/>
   <br/>
<br/>
