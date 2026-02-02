# Editing the Dev Blog
This is a short tutorial on how to add blog posts to the website and editing content without cloning/pulling from the gh-pages branch and possibly messing things up. Everything important can be handled within the branch directory itself without having to pull and push new files.

All blog posts are written in Markdown and are processed into HTML through the Jekyll templating engine. Please do not try to hardcode & hardlink an HTML page into the blog; **the website will probably get messed up.**
## Editing Member Information
Go to the **_data** folder and open the **members.yml** file. Change information as desired. Pretty simple

## Making a Blog Post
Go to the **_posts** folder and select **create new file**.

**Important:** Naming conventions for post files are very specific, and I don't want to risk broken links as the blog builds & redeploys. When creating a new post, follow this convention: ` YYYY-MM-DD-filename.md `

The file **must** have a date in YYYY-MM-DD format preceding any other information, and it must also be a Markdown (md) file.

### Making the Post
To render as an HTML page correctly, there is additional non-negotiable front matter that must be included in every post file. Front matter is defined at the very start of the markdown file between two lines of three dashes (---). They are, in order:

- Layout of type 'post'
    - The layout field MUST be of type 'post' to render as a blog article. It will fail to build with an error otherwise.
- Title with double-quotation marks around content
- Date in YYYY-MM-DD format
- Tag(s)
- Category(ies)
- Author name

An example of a blog's front matter is shown below:

```md
---
layout:     post
title:      "Example"
date:       1970-01-01
tag:        tag-name
category:   category-name
author:     John Doe
---
```

For tags and categories: at least **one** term for each field must be present, but there may be multiple valid terms, separated by whitespace. A single tag term will be defined with a singular tag; see below:
```
tag: design
```
or
```
tags: design notes process
```

The same convention exists for categories. 

For the sake of brevity, I'd like to limit categories to maybe three or four: "design", "development", and "nondev", and whatever might arise when we need it. This is due to how Jekyll processes categories: they're all subfolders of the greater archive and I'd like to avoid having ten folders of arbitrary classifications.

Fortunately, tags do not suffer this issue and may be used freely. Make as many tags as you want! Users can search through the blog by either tag or category.

Alternatively, if you don't want to post the blog entry you have just yet, you can place the file in the **_drafts** folder so Jekyll does not render the blog post as a page on build.

### Adding Content
#### Text
Good news: adding text into Markdown files is supremely uncomplicated. You can type as normal anywhere below the front matter, and this will be rendered as the blog content. You do not need to use HTML tags or attributes; everything from this point on is in pure Markdown.

```md
---
layout:     post
title:      "Example"
date:       1970-01-01
tag:        tag-name
category:   category-name
author:     John Doe
---

I am the content of this blog post and will render as regular HTML! What you see is what you'll get!
# Big heading
## Less big heading
### You get the point
```
#### Images and Other Files
First, go to the **assets** folder. This is where any static files (files that do not get processed through Jekyll on build) go to be stored and referenced by other files. There are three subfolders, /css/, /files/, and /images/. Just concern yourself with /files/ and /images/ for now. 

Upload any images in the images folder and any non-image files (PDFs, etc.) into the files folder. Their relative path should be something like `/assets/folder/file.ex`. This is the link you'd use to insert media into a blog post.

Images in Markdown are formatted like this:

```md
![Image Alt Text](/assets/folder/file.jpg)
```

and links are formatted like this:
```md
[Link Text](/assets/folder/file.pdf)
```

and, uh, yeah. That's pretty much it. For more in-depth information, consult the [Jekyll Posts Docs](https://jekyllrb.com/docs/posts/). Happy posting!

-Nao