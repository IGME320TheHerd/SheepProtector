---
layout:     post
title:      "Designing UI - Design Process"
date:       2026-02-26
tag:        design ui/ux
category:   design
author:     Nao Belgrave
---

Apart from project organization and creating our development blog, my primary role in this project was designing and implementing UI, which involved designing the layout of menus, modals, and loading screens.

## Gathering Inspiration
At first, I looked at [ABZU](https://store.steampowered.com/app/384190/ABZU/) and [The Last Campfire](https://store.steampowered.com/app/990630/The_Last_Campfire/) as references: they both tended to utilize a basic black overlay with some high contrast text on top for pause, settings, and main menu screens. I generally wanted something simple that wouldn't draw the player's attention out of the experience too much, and I liked how these two games really exemplified that in their UI design.

But... wait. What about the settings menu?

I could have a entirely separate settings screen, just for ease of development, but that would require a dedicated keybind for both KBM and controller and establishing this kind of summative development precedent could be at odds with our design pillars. It would also mess with the player's mental model of a pause screen-- most game pause screens have a subdirectory of settings, credits, etc.,

I then came across [this](https://medium.com/@a_kill_/video-game-ui-the-design-process-explained-14c23fa70d37) Medium article by [Akhil Dakinedi](https://medium.com/@a_kill_), who writes extensively on all manners of UI, especially game UI. He breaks down his process quite succinctly in this article, and he offered some design alternatives to the traditional three-button menu.

Dakinedi's detailed iterative process also made me realize that I was not making *nearly* as many mockups as I should've been. So, it's back to the drawing board.
## First Passes
After going through my inspirations, which I talk about in more depth [here](2026-02-04-references.md), I created a couple mockups in Figma for the pause menu screen, which look like this. The background image I use here is concept art provided by our main artist, Milo Qureshi.

![Images of rough concepts]({{ site.baseurl }}/images/pauseconcepts.png)

Both are pretty simple, admittedly, but they both encapsulate the "feel" that I want from this pause menu: a minimalist, unobtrusive display that shows only the things that the user needs, without any embellishments. The arrow is the only pop of color in a monochrome menu, immediately drawing the user's eye to their selection. Perhaps the text could be highlighted as well...? (But that's a thought for later.)

## Implementation
All of this UI design, or at least the foundation, were required for our MVI build due around two weeks after we started development. With my rough concepts done, I got to work implementing them in Unity, which was pretty straightforward. I was used to working with Unity's in-scene Canvas UI, and it's honestly my preferred UI implementation since programming in Unity events is much, much easier.

When I was finished, the pause menu looked like this:

![Screenshot of the rough pause menu implemented in Unity.]({{ site.baseurl }}/images/pausedevss.png)

Which, okay! It's nowhere near as refined as my first draft, but the idea is there. The text is mostly in the same place, but the buttons are not. This was a conscious decision I made due to the differences in interaction between KBM and controller. Currently, we're only really developing for KBM while keeping controller inputs in the back of our minds, so I decided that it'd be most useful to focus on KBM for our first build.