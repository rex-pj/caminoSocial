import React, { Fragment, useState } from "react";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import AsyncSelect from "react-select/async";
import { PrimaryTextbox } from "../../atoms/Textboxes";
import styled from "styled-components";

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
  .cate-selection {
    > div {
      border: 1px solid ${(p) => p.theme.color.primaryDivide};
    }
  }
`;

export default function (props) {
  const { data, execution } = props;
  const { onEditAttribute } = execution;
  const { attribute, index } = data;
  const [formData, setFormData] = useState(attribute);

  const handleInputChange = (evt) => {
    let attribute = formData;
    const { name, value } = evt.target;
    attribute[name] = value;

    setFormData({ ...attribute });
  };

  const editAttribute = () => {
    onEditAttribute(formData, index);
    props.closeModal();
  };

  const { displayOrder } = formData;
  return (
    <Fragment>
      <PanelBody>
        <FormRow>
          <AsyncSelect
            className="cate-selection"
            placeholder="Select attribute"
          />
        </FormRow>
        <FormRow>
          <AsyncSelect
            className="cate-selection"
            placeholder="Select control type"
          />
        </FormRow>
        <FormRow>
          <PrimaryTextbox
            name="displayOrder"
            value={displayOrder ? displayOrder : ""}
            placeholder="Display Order"
            onChange={handleInputChange}
          />
        </FormRow>
      </PanelBody>
      <PanelFooter>
        <ButtonPrimary onClick={() => editAttribute()} size="xs">
          {index || index === 0 ? "Update" : "Add"}
        </ButtonPrimary>
      </PanelFooter>
    </Fragment>
  );
}