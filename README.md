# WordPredictionLibrary
-Tracks the frequency of words that follow other words.
-You must train up the dictionary by supplying it with a body of text.
-Stores each word seen, and each word that follows each given word.
-Allows for serializing/deserializing of entire data structure.
-Stores the raw word counts, rather than a percentage or probability, because a running probability would introduce an ever-increasing error.
-Many diffrent possible predictions can be drawn from the data:
   -Simple next-word probability or suggestion
   -Analyzing a body of text for randomness by examining its next-word probabilies.
   -Word collocation analysis
   -Single/Unique word analysis
   
