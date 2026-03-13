---
layout:     post
title:      "Taking From Others - User Interaction & Experience"
date:       2026-02-04
tag:        design process
category:   design
author:     Nao Belgrave
---
"A good artist copies, a great artist steals" is a quote famously attributed to Pablo Picasso. Whether or not he actually said this is up to historical record, but the point of this oft-repeated quote is how artists, and any other creative for that matter, should interact with the works of other artists and creatives. A good, run-of-the-mill creative superficially mimics the results of others' work, while a great creative will take into account the foundation underneath that polished result and apply the process to their own, building up an experience from the ground up.

This philosophy, of course, applies to game design as well, so part of our pre-development research and organization on Sheep Protector involved going out and finding games that successfully implemented themes, mechanics, or visual design that we wanted to have in Sheep Protector.

I worked mostly on designing and implementing UI and UX for the game, so most of the games I cite employ similar menus, HUDs, and avenues for player interaction that I want to implement in Sheep Protector. I already had a couple games in mind when considering what I wanted the UI to look like, but for further research, I headed over to the [Game UI Database](https://www.gameuidatabase.com/index.php) and [Interface In Game](https://interfaceingame.com/), two database websites that document the various moving parts in game UI and UX design.

I also wanted to focus on games made for console: we were responsible for developing for both KBM (keyboard and mouse) and controller input, and since I was vastly more familiar with the ways UI could be designed for KBM, I decided to start with console games as my primary sources of inspiration.

## RiME (2017)
As a semi-open world 3D puzzler, RiME has virtually no UI to speak of, apart from simple button pop-ups used to onboard a small array of player actions. For a visually rich game aiming for maximum immersion, this is no surprise, but the real magic is hidden in the ways that UI/UX pop up in-game.

There are a couple of player actions in the game with visual feedback, notably the player protagonist's shouting action, which will activate and light up any relevant objects nearby, resulting in a visually stunning and informative sequence that illuminates the path ahead while guiding the player to their next objective.

For puzzle or quest objectives, RiME tends to use these beacon-like light spires to signal to the player, wherever they are on the map, that there is something important at the source of this light. 

![Screenshot of the first level of RiME]({{ site.baseurl }}/images/rime.png)

Since RiME is a puzzle-adventure game, there are several other objects and buildings in the landscape that are similarly illuminated. For a simpler exploration-adventure game like our Sheep Protector, all these bells and whistles are not necessarily required, but they still serve as good examples of indicating player actions without breaking from the in-game environment and communicating with the player directly, the latter of which I was steadfast on implementing and maintaining.
## ABZU (2016)
Like RiME, ABZU is also a semi-open world puzzler with very minimal UI. I was most attracted to ABZU for how it onboards player actions, as seen below. 

![Screenshot of a player attempting to ride a fish in ABZU]({{ site.baseurl }}/images/abzu.png.jpg)

Onboarding is one of those things that can make or break a game. Be too vague, and the player is upset at the lack of direction; be too detailed, and the player becomes exhausted from the information constantly interrupting their experience.

At first, I was vehemently against the kind of explicit onboarding seen here in ABZU. I asked myself, "Wouldn't the player get taken out of the game experience from this?" 

I was pretty worried about how we could teach the player what to do and how to do it without "breaking immersion", but ABZU manages to blend the instructions in quite nicely: since the player is likely already confused on what to do next, getting simple visual instructions is the most efficient way for the player to understand their next steps without prolonging their bewilderment.

Emotions, too, draw players out of the game experience; this is something I ought to remember going forward.
## Shin Megami Tensei IV (2013)
I was in the process of playing through SMT IV while gathering resources for the UI design, and I wanted to pay a small homage to a game that I had been having quite a lot of fun playing, which ended up being much easier than I thought. 

SMT IV, like earlier SMT entries, utilizes this top-down map exploration system to allow the player to traverse large swathes of the map very quickly, as opposed to the over-the-shoulder 3rd-person perspective consistent with the combat views and close-quarters exploration of the rest of the game. In SMT, the player's way of dealing with enemies was limited to dodging them or facing them head on, both of which can be pretty stressful!

![Screenshot of the Tokyo map in Shin Megami Tensei 4]({{ site.baseurl }}/images/smt4.png)

Sheep Protector, too, uses these interactions as the core mechanics of the game. Thus, SMT's overworld map additionally served as a model for how our map layout and player interactions should work. The tension and stress the player feels navigating through SMT is something that we wanted to achieve in our own game, albeit to a slightly lesser extent.

SMT also uses some interesting UI to signal information to the user: available paths are illuminated, as shown in the example above, and interactables are designated by a bright yellow square with an arrow inside. While Sheep Protector does not feature the post-apocalyptic environment standard to SMT games, a similar indicator may be used for player interactables around the map.