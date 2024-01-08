Note: I made some simplifications / shortcuts for the sake of time. I'll list them below.

Single controller: Kept everything under one roof because, honestly, for a CRUD app, splitting into multiple controllers or getting into CQRS just felt like overkill.

Domain and DB entities, I didn’t bother separating them. For something this straightforward, it seemed unnecessary. This is a simple app, not some complex system.

Error handling: The middleware I used is pretty basic. It’s not aggregating a bunch of errors, just deals with the immediate one. Thought that was good enough for now.

Repositories are there, but no services wrapping them. Felt like adding another layer would of indirection for such a simple app is just overkill. On a real project, I’d probably have a service layer, but for this, it just felt like a waste.

Also, bits to mention:

For the ActorId, I used the entire href string, this isn't because of lack of ability, I just simply don't believe the intent of the exercise is to see if I can split a string.

Added a /actors/populate endpoint. Made it a GET for easy browser triggering. I know, I know, it's not the best REST practice. In a real app, I'd probably have it as a background task or something, but this was just simpler for now.

Validation: I used FluentValidation. Didn’t integrate it with ASP.NET Core middleware because, apparently, that’s not the recommended way anymore. I just injected the validator directly. It's manageable for now, but for a bigger project, I'd probably need a different approach.

TraceId: I didn't bother generating them per request so I just generated them randomly as a simplification.

Similarly, I used the DTO request object in the repository option, this is again just a shortcut to not have to create another class. In a real app, I'd probably have a separate class for the repository options.