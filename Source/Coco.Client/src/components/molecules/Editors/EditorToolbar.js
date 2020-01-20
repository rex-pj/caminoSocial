import React, { useEffect } from "react";
import styled from "styled-components";
import { EditorState, Modifier } from "draft-js";
import { DefaultButton } from "./EditorButtons";
import EditorDropdown from "./EditorDropdown";

const Toolbar = styled.div`
  padding: ${p => p.theme.size.tiny};
  border-bottom: 1px solid ${p => p.theme.color.light};
  background-color: ${p => p.theme.color.lighter};
`;

const EditorButton = styled(DefaultButton)`
  border: 0;
  margin-right: 4px;
  :hover {
    background-color: ${p => p.theme.color.light};
  }

  &.actived {
    background-color: ${p => p.theme.color.light};
  }
`;

const Divide = styled.span`
  display: inline-block;
  border-right: 1px solid ${p => p.theme.color.light};
  height: 20px;
  width: 0px;
  margin: 0 8px 0 4px;
  vertical-align: middle;
`;

const SelectHeading = styled(EditorDropdown)`
  background-color: ${p => p.theme.rgbaColor.light};
  color: ${p => p.theme.color.primaryLight};
  font-weight: 600;
  border: 0;

  :hover {
    background-color: ${p => p.theme.color.light};
  }
`;

export default props => {
  const { editorState, inlineTyles, blockTyles, headingTypes } = props;

  const toggleBlockType = e => {
    const value = e.target.value;
    props.toggleBlockType(value);
    focusEditor();
  };

  const toggleInlineStyle = e => {
    const value = e.target.value;
    props.toggleInlineStyle(value);
  };

  const currentStyle = editorState.getCurrentInlineStyle();
  const selection = editorState.getSelection();
  const blockType = editorState
    .getCurrentContent()
    .getBlockForKey(selection.getStartKey())
    .getType();

  const focusEditor = () => {
    setTimeout(() => {
      props.focusEditor();
    }, 50);
  };

  function removeInlineStyles(currentState) {
    const contentState = currentState.getCurrentContent();
    const contentWithoutStyles = inlineTyles.reduce(
      (state, item) =>
        Modifier.removeInlineStyle(
          state,
          currentState.getSelection(),
          item.style
        ),
      contentState
    );

    return EditorState.push(
      currentState,
      contentWithoutStyles,
      "change-inline-style"
    );
  }

  function removeBlockStyles(currentState) {
    const contentState = currentState.getCurrentContent();
    let newEditorState = currentState;
    let contentWithoutStyles = contentState;
    contentWithoutStyles = Modifier.setBlockType(
      contentWithoutStyles,
      currentState.getSelection(),
      "unstyled"
    );

    return EditorState.push(
      newEditorState,
      contentWithoutStyles,
      "change-block-type"
    );
  }

  const clearFormat = () => {
    const helpers = [];

    helpers.push(removeInlineStyles);

    helpers.push(removeBlockStyles);

    const newEditorState = helpers.reduce(
      (state, helper) => helper(state),
      editorState
    );

    props.clearFormat(newEditorState);
  };

  useEffect(() => {
    return () => {
      return clearTimeout();
    };
  });

  return (
    <Toolbar>
      {blockTyles.map(type => (
        <EditorButton
          key={type.style}
          actived={type.style === blockType}
          label={type.label}
          icon={type.icon}
          onToggle={toggleBlockType}
          style={type.style}
        />
      ))}
      <Divide />
      {inlineTyles.map(type => (
        <EditorButton
          key={type.style}
          actived={currentStyle.has(type.style)}
          label={type.label}
          icon={type.icon}
          onToggle={toggleInlineStyle}
          style={type.style}
        />
      ))}
      <Divide />
      <SelectHeading
        options={headingTypes}
        actived={blockType}
        onToggle={toggleBlockType}
        placeholder="Heading Styles"
      />
      <EditorButton icon="eraser" onToggle={clearFormat} />
    </Toolbar>
  );
};
