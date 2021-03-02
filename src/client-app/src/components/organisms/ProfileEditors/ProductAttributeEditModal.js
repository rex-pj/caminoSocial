import React, { Fragment } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary, ButtonLight } from "../../atoms/Buttons/Buttons";

export default function (props) {
  const { children } = props;

  const onDeletting = () => {
    const { data, execution } = props;
    const { id } = data;
    const { onDelete } = execution;
    onDelete(id);
    props.closeModal();
  };

  return (
    <Fragment>
      <PanelBody>{children}</PanelBody>
      <PanelFooter>
        <ButtonPrimary onClick={onDeletting} size="xs">
          Delete
        </ButtonPrimary>
        <ButtonLight onClick={() => props.closeModal()} size="xs">
          Cancel
        </ButtonLight>
      </PanelFooter>
    </Fragment>
  );
}
